using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebAppPI03.Models;

namespace WebAppPI03.Controllers
{
    public class PesquisaController : Controller
    {
        // GET: Pesquisa
        //Pesquisa
        public ActionResult Index(string pesquisar)
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("SELECT nomeProduto precProduto, imagem FROM Produto WHERE nomeProduto LIKE '%{0}%' OR descProduto  LIKE '%{0}%'", pesquisar);

            var query = db.Database.SqlQuery<Produto>(sb.ToString()).ToList();
            if (query.Count() > 0)
            {
                return View();
            }
            else
            {
                ViewBag.Resultado = "Nenhum produto encontrado.";
                return View();
            }

        }
    }
}