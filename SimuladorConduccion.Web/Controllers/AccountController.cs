using SimuladorConduccion.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimuladorConduccion.Web.ViewModels;


namespace SimuladorConduccion.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly LoginService _loginService;

        public AccountController()
        {
            _loginService = new LoginService();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuario = _loginService.ValidarUsuario(
                model.NombreUsuario,
                model.Contrasena
            );

            if (usuario == null)
            {
                model.Error = "Usuario o contraseña incorrectos";
                return View(model);
            }

            // Guardar sesión básica
            Session["UsuarioId"] = usuario.UsuarioId;
            Session["NombreUsuario"] = usuario.NombreUsuario;

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}