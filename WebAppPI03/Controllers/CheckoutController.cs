using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppPI03.Models;

namespace WebAppPI03.Controllers
{
    public class CheckoutController : Controller
    {

        private EntidadesEcommerce db = new EntidadesEcommerce();

        [Authorize]
        public ActionResult Index()
        {
            if (Session["Carrinho"] != null)
            {
                List<ItemCarrinho> listaCarrinho = new List<ItemCarrinho>();
                listaCarrinho = (List<ItemCarrinho>)Session["Carrinho"];
                if (listaCarrinho.Count == 0)
                {
                    return RedirectToAction("Carrinho", "Produto");
                }
                return View();
            }
            return RedirectToAction("Carrinho", "Produto");
        }

        public ActionResult checkOutCliente()
        {
            Validar vd = new Validar();

            var id = vd.getIDControl();
            var first = db.Cliente.Where(m => m.idCliente == id).FirstOrDefault();

            return PartialView(first);
        }

        public ActionResult checkOutEndereco()
        {
            Validar vd = new Validar();

            var id = vd.getIDControl();
            var first = db.Endereco.Where(m => m.idCliente == id).ToList();

            return PartialView(first);
        }
        public JsonResult checkOutEnderecoUni(int id)
        {
            if (id != 0)
            {
                Endereco end = db.Endereco.Where(m => m.idEndereco == id).FirstOrDefault();
                ViewBag.Resultado = end;
                return Json(new
                {
                    endereco = end.nomeEndereco,
                    cep = end.CEPEndereco,
                    compl = end.complementoEndereco,
                    noob = end.numeroEndereco
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);

        }

        #region Boleto
        [Authorize]
        [HttpPost]
        public ActionResult PagamentoBoleto(int endereco)
        {
            Validar vd = new Validar();
            DateTime date = DateTime.Today;
            bool blnRet = false;


            var status = db.StatusPedido.Where(p => p.idStatus == 2).FirstOrDefault();
            var TipoPagamento = db.TipoPagamento.Where(t => t.idTipoPagto == 2).FirstOrDefault();
            var aplicacao = db.Aplicacao.Where(a => a.idAplicacao == 0).FirstOrDefault();

            Pedido ped = new Pedido();
            ped.idCliente = vd.getIDControl();
            ped.idEndereco = endereco;
            ped.idStatus = status.idStatus;
            ped.idTipoPagto = TipoPagamento.idTipoPagto;
            ped.idAplicacao = aplicacao.idAplicacao;
            ped.dataPedido = date;
            db.Pedido.Add(ped);
            db.SaveChanges();

            int idPedido = ped.idPedido;

            List<ItemCarrinho> listaCarrinho = new List<ItemCarrinho>();

            if (Session["Carrinho"] != null)
            {
                listaCarrinho = (List<ItemCarrinho>)Session["Carrinho"];
            }

            if (listaCarrinho != null || listaCarrinho.Count != 0)
            {
                foreach (var produto in listaCarrinho)
                {
                    var qtd = int.Parse(produto.productQtd.ToString());
                    if (qtd > 0)
                    {
                        var idProduto = int.Parse(produto.productId.ToString());
                        var resultado = db.Produto.Where(m => m.idProduto == idProduto).FirstOrDefault();

                        ItemPedido item = new ItemPedido();
                        item.idProduto = idProduto;
                        item.qtdProduto = Convert.ToInt16(qtd);
                        resultado.descontoPromocao = resultado.descontoPromocao == null ? 0 : resultado.descontoPromocao;
                        item.precoVendaItem = resultado.precProduto - (decimal)resultado.descontoPromocao;
                        item.idPedido = idPedido;

                        try
                        {
                            db.ItemPedido.Add(item);
                            db.SaveChanges();
                            blnRet = true;
                        } 
                        catch (Exception)
                        {
                            blnRet = false;
                        }

                        if (blnRet)
                            return View("Confirmado");
                        else
                            return View("Falha");
                    }
                }
            }

            return View("Index");
        }
        #endregion

        #region Cartao
        [Authorize]
        [HttpPost]
        public ActionResult PagamentoCartao(int endereco, string txtImpressao, string txtCartao, string txtCodigo)
        {
            Validar vd = new Validar();
            DateTime date = DateTime.Today;
            bool blnRet = false;
            Cartao ct = new Cartao();
            List<ItemCarrinho> listaCarrinho = new List<ItemCarrinho>();

            if (Session["Carrinho"] != null)
            {
                listaCarrinho = (List<ItemCarrinho>)Session["Carrinho"];
            }

            decimal total;
            total = 0m;

            foreach (var produto in listaCarrinho)
            {
                var idProdutot = int.Parse(produto.productId.ToString());
                var resultadot = db.Produto.Where(m => m.idProduto == idProdutot).FirstOrDefault();

                var qtd = int.Parse(produto.productQtd.ToString());
                resultadot.descontoPromocao = resultadot.descontoPromocao == null ? 0 : resultadot.descontoPromocao;
                total = total + resultadot.precProduto - (decimal)resultadot.descontoPromocao;
            }

            bool pode = ct.Verificar(txtImpressao, txtCartao, Convert.ToString(total), txtCodigo);
            if (pode)
            {

                var status = db.StatusPedido.Where(p => p.idStatus == 3).FirstOrDefault();
                var TipoPagamento = db.TipoPagamento.Where(t => t.idTipoPagto == 1).FirstOrDefault();
                var aplicacao = db.Aplicacao.Where(a => a.idAplicacao == 0).FirstOrDefault();

                Pedido ped = new Pedido();
                ped.idCliente = vd.getIDControl();
                if (endereco != 0)
                    ped.idEndereco = endereco;
                else
                    ped.idEndereco = 0;
                
                ped.idStatus = status.idStatus;
                ped.idTipoPagto = TipoPagamento.idTipoPagto;
                ped.idAplicacao = aplicacao.idAplicacao;
                ped.dataPedido = date;
                db.Pedido.Add(ped);
                db.SaveChanges();

                int idPedido = ped.idPedido;

                if (listaCarrinho != null || listaCarrinho.Count != 0)
                {
                    foreach (var produto in listaCarrinho)
                    {
                        var qtd = int.Parse(produto.productQtd.ToString());
                        if (qtd > 0)
                        {
                            var idProduto = int.Parse(produto.productId.ToString());
                            var resultado = db.Produto.Where(m => m.idProduto == idProduto).FirstOrDefault();

                            ItemPedido item = new ItemPedido();
                            item.idProduto = idProduto;
                            item.qtdProduto = Convert.ToInt16(qtd);
                            resultado.descontoPromocao = resultado.descontoPromocao == null ? 0 : resultado.descontoPromocao;
                            item.precoVendaItem = resultado.precProduto - (decimal)resultado.descontoPromocao;
                            item.idPedido = idPedido;

                            try
                            {
                                db.ItemPedido.Add(item);
                                db.SaveChanges();
                                blnRet = true;
                            }
                            catch (Exception)
                            {
                                blnRet = false;
                            }

                            if (blnRet)
                                return View("Confirmado");
                            else
                                return View("Falha");
                        }
                    }
                }
            }
            return View("Index");
        }
        #endregion

        public ActionResult Confirmado()
        {
            return View();
        }

        public ActionResult Falha()
        {
            return View();
        }


    }
}