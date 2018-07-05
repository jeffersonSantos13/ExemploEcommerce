using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebAppPI03.Models;
using System.Collections.Generic;

namespace WebAppPI03.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        #region Validadores
        [AllowAnonymous]
        public JsonResult VerificarEmail(string emailCliente)
        {
            Validar vad = new Validar();
            bool blnRes = vad.isUniqueEmail(emailCliente);
            if (blnRes)
                return Json("E-mail já cadastrado.",
                    JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult isCPFValido(string CPFCliente)
        {
            Validar vad = new Validar();
            bool blnRes = vad.isCPFValido(CPFCliente);
            if (blnRes == false)
                return Json("CPF inválido.",
                    JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult isUniqueCPF(string CPFCliente)
        {
            Validar vad = new Validar();
            bool blnRes = vad.isUniqueCpF(CPFCliente);
            if (blnRes == false)
                return Json("CPF já cadastrado.",
                    JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            Validar vd = new Validar();
            if (vd.ChecarAcesso(login))
            {
                if (Autenticar(login, returnUrl))
                    ViewBag.Resultado = "Seja beeem vindo !";
                if (returnUrl != null)
                {
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (login.Email != null && login.Senha != null)
                {
                    ViewBag.Resultado = "Email ou senha invalido";
                }
            }
            return View();
        }
        private bool Autenticar(LoginViewModel login, string returnUrl)
        {

            var claims = new List<Claim>();
            EntidadesEcommerce db = new EntidadesEcommerce();
            var first = db.Cliente.Where(m => m.emailCliente == login.Email).FirstOrDefault();
            claims.Add(new Claim(ClaimTypes.Email, login.Email));
            claims.Add(new Claim(ClaimTypes.Role, "Cliente"));
            claims.Add(new Claim(ClaimTypes.Name, first.nomeCompletoCliente));
            claims.Add(new Claim(ClaimTypes.UserData, Convert.ToString(first.idCliente)));
            var id = new ClaimsIdentity(claims,
                    DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            authenticationManager.SignIn(id);
            return true;


        }

        public ActionResult LogoOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            Session.Clear();
            return RedirectToAction("index", "Home");
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Cliente model, string confPassword)
        {
            if (ModelState.IsValid)
            {
                if (model.senhaCliente == confPassword)
                {
                    EntidadesEcommerce db = new EntidadesEcommerce();
                    db.Cliente.Add(model);
                    db.SaveChanges();
                    
                    ViewBag.Resultado = "Usuario cadastrado com sucesso !";
                    return View("Login");


                }
                else
                {
                    ViewBag.Resultado = "As senhas não são iguais!";
                    return View();
                }

            }
            else
            {
                ViewBag.Resultado = "Ocorreu um erro durante seu cadastro!";
                return View();
            }
            
        }
        [AllowAnonymous]
        //POST//Registrar
        public ActionResult RegisterEndPartialView()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public ActionResult _LoginPartial()
        {

            return PartialView();
        }
       

    }
}