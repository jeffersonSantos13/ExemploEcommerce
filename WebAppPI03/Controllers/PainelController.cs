using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppPI03.Models;

namespace WebAppPI03.Controllers
{
    [Authorize]
    public class PainelController : Controller
    {
        // GET: Painel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialClienteView()
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            Validar vb = new Validar();
            int id = vb.getIDControl();
            
            var listCliente = db.Cliente.Where(m => m.idCliente == id).FirstOrDefault();
            return PartialView(listCliente); 
        }

        public ActionResult PartialPedidoView()
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            Validar vb = new Validar();
            int id = vb.getIDControl();
            var listPed = db.Pedido.Where(a => a.idCliente == id).ToList();
            return PartialView(listPed);
        }

       
        public ActionResult PartialEnderecoView()
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            Validar vb = new Validar();
            int id = vb.getIDControl();
            var listEnd = db.Endereco.Where(m => m.idCliente == id).ToList();
            return PartialView(listEnd);
        }
        //Funfa n
        public ActionResult DelEnd(int id)
        {
            Validar vd = new Validar();
            vd.DeleteEnd(id);
            return View("/Painel/Index");
        }

        public ActionResult RegisterEnd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterEnd(Endereco end)
        {
            EntidadesEcommerce db = new EntidadesEcommerce();
            Validar vd = new Validar();
            end.idCliente = vd.getIDControl();
            db.Endereco.Add(end);
            db.SaveChanges();
            return View("Index");
        }
    }
}