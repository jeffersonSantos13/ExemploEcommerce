using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebAppPI03.Models;

namespace WebAppPI03.Controllers
{
    public class ProdutoController : Controller
    {
        public object resultado { get; set; }
        private EntidadesEcommerce db = new EntidadesEcommerce();

        //Descrição de produtos
        public ActionResult descProduto(int id = 0)
        {
            if (db.Produto.Where(p => p.idProduto == id).Count() > 0)
            {
                var resultado = db.Produto.Where(p => p.idProduto == id).FirstOrDefault();
                return View(resultado);
            }
            else
            {
                return View("produtoNaoEncontrado");
            }
        }

        public ActionResult PartialRecomendado(int id)
        {
            var categoria = db.Produto.Where(m => m.idProduto == id).FirstOrDefault();
            var prodRecent = db.Produto.Where(m => categoria.idCategoria == m.idCategoria && m.ativoProduto == "1").Take(2).ToList();
            return PartialView();
        }

        // GET: Produto
        public ActionResult ListaProd(int id = 0, int at = 0, int order = 0)
        {
            if (id == -1)
            {
                return View(db.Produto.Where(p => p.idCategoria > 5).OrderBy(o => o.idCategoria).ToList());
            }

            if (db.Categoria.Where(p => p.idCategoria == id).Count() > 0)
            {
                ViewBag.Mensagem = db.Categoria.Find(id).nomeCategoria.ToString();
                return View(db.Produto.Where(p => p.idCategoria == id && p.ativoProduto == "1" && p.idProduto >= at).Take(4).ToList());
            }
            else
            {
                return View(db.Produto.ToList());
            }
        }
        public JsonResult ListaProdMais(int id, int at = 0)
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            var lista = db.Produto.Where(m => m.idCategoria == id && m.ativoProduto == "1" && m.idProduto > at).OrderBy(m => m.nomeProduto).Take(4).ToList();
            string b64;
            Produto[] resultado = new Produto[lista.Count()];
            for (int i = 0; i < lista.Count(); i++)
            {
                if (lista[i].imagem == null)
                {
                    b64 = "0";
                }
                else
                {
                    b64 = Convert.ToBase64String(lista[i].imagem);
                }

                resultado[i] = new Produto();
                resultado[i].idProduto = lista[i].idProduto;
                resultado[i].nomeProduto = lista[i].nomeProduto;
                resultado[i].imagem64 = b64;
                resultado[i].descProduto = lista[i].descProduto;
                resultado[i].precProduto = lista[i].precProduto;
                resultado[i].descontoPromocao = lista[i].descontoPromocao;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListaProdRequest(int id, int order = 0, int at = 0)
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            List<Produto> lista;
            switch (order)
            {
                case 1:
                    lista = db.Produto.Where(m => m.idCategoria == id && m.ativoProduto == "1" && m.idProduto >= at).OrderByDescending(m => m.idProduto).Take(3).ToList();
                    break;
                case 2:
                    lista = db.Produto.Where(m => m.idCategoria == id && m.ativoProduto == "1" && m.idProduto >= at).OrderBy(m => m.nomeProduto).Take(3).ToList();
                    break;
                case 3:
                    lista = db.Produto.Where(m => m.idCategoria == id && m.ativoProduto == "1" && m.idProduto >= at).OrderByDescending(m => m.precProduto).Take(3).ToList();
                    break;
                case 4:
                    lista = db.Produto.Where(m => m.idCategoria == id && m.ativoProduto == "1" && m.descontoPromocao > 0 && m.idProduto >= at).OrderBy(m => m.nomeProduto).Take(3).ToList();
                    break;
                default:
                    lista = db.Produto.Where(m => m.idCategoria == id && m.ativoProduto == "1" && m.idProduto >= at).OrderBy(m => m.nomeProduto).Take(3).ToList();
                    break;
            }
            string b64;
            Produto[] resultado = new Produto[lista.Count()];
            for (int i = 0; i < lista.Count(); i++) {
                if(lista[i].imagem == null)
                {
                    b64 = "0";
                }else
                {
                    b64 = Convert.ToBase64String(lista[i].imagem);
                }
               
                resultado[i] = new Produto();
                resultado[i].idProduto = lista[i].idProduto;
                resultado[i].nomeProduto = lista[i].nomeProduto;
                resultado[i].imagem64 = b64;
                resultado[i].descProduto = lista[i].descProduto;
                resultado[i].precProduto = lista[i].precProduto;
                resultado[i].descontoPromocao = lista[i].descontoPromocao;
            }

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        //Destaque principal (Banner grandão)
        public ActionResult PartialDestaqueGrande()
        {

            int intQtd = 0;

            Random r = new Random();
            int intNum = r.Next(0, intQtd);
            intQtd = db.Produto.Where(a => a.imagem != null && a.ativoProduto == "1").Count();
            var prod = db.Produto.Where(a => a.imagem != null && a.ativoProduto == "1").OrderBy(p => p.idProduto).Skip(intNum).FirstOrDefault();

            return PartialView(prod);

        }

        //Destaque recentes (Banner pequenos)
        public ActionResult PartialDestaque()
        {
            var lista = db.Produto.Where(a => a.imagem != null && a.ativoProduto == "1").OrderByDescending(p => p.idProduto).Take(8).ToList();
            return PartialView(lista);
        }

        //FIltro
        public ActionResult Filtro()
        {
            return PartialView();
        }

        public ActionResult Consultar(string pesquisar)
        {
            if (!string.IsNullOrEmpty(pesquisar))
                ViewBag.Mensagem = "Resultado da Busca por: '" + pesquisar + "'";
            var resultado = db.Produto.Where(s => s.nomeProduto.Contains(pesquisar) || s.descProduto.Contains(pesquisar));
            return View("ListaProd", resultado);
        }

        [HttpPost]
        public JsonResult Carrinho(string cart)
        {
            List<ItemCarrinho> cartList = new List<ItemCarrinho>();

            if (cart != null)
            {
                var produtos = JsonConvert.DeserializeObject<List<ItemCarrinho>>(cart);

                foreach (var produto in produtos)
                {
                    var qtd = int.Parse(produto.productQtd.ToString());
                    if (qtd > 0)
                    {
                        var idProduto = int.Parse(produto.productId.ToString());
                        var resultado = db.Produto.Where(m => m.idProduto == idProduto).FirstOrDefault();

                        ItemCarrinho prod = new ItemCarrinho();
                        prod.productId = idProduto;
                        prod.productQtd = qtd;
                        prod.productName = resultado.nomeProduto;
                        resultado.descontoPromocao = resultado.descontoPromocao == null ? 0 : resultado.descontoPromocao;
                        prod.productPrice = resultado.precProduto - (decimal)resultado.descontoPromocao;
                        prod.productTotal = qtd * prod.productPrice;
                        string imgProduct = string.Empty;
                        if (resultado.imagem != null)
                        {
                            string base64String = Convert.ToBase64String(resultado.imagem);
                            imgProduct = "data:image/png;base64," + base64String;
                        }
                        else
                        {
                            imgProduct = "/Content/Imagens/produto-sem-imagens.gif";
                        }
                        prod.productImage = imgProduct;
                        cartList.Add(prod);
                    }
                }
                Session["Carrinho"] = cartList;
            }
            return Json(JsonConvert.SerializeObject(cartList));
        }

        public ActionResult Carrinho()
        {
            ViewBag.IsCartPage = true;
            return View();
        }

    }
}