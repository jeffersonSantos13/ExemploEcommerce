using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppPI03.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NaoEncontrada()
        {
            return View();
        }
        public ActionResult NaoAutorizado()
        {
            return View();
        }
        public ActionResult Esgotado()
        {
            return View();
        }
    }
}