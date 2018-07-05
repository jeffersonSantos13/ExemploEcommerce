using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPI03.Models;

namespace WebAppPI03.Controllers
{
    public class CategoriaController : Controller
    {
        // GET: Categoria
        public ActionResult PartialCategoria()
        {
            EntidadesEcommerce db = new EntidadesEcommerce();

            var lista = db.Categoria.OrderBy(m => m.nomeCategoria).ToList();

            return PartialView(lista);

        }
    }
}