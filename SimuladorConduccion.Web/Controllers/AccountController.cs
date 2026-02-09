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
        private readonly LoginService _loginService = new LoginService();

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var user = _loginService.Validate(model.Username, model.Password);

            if (user == null)
            {
                model.Error = "Usuario o contraseña incorrectos";
                return View(model);
            }

            Session["User"] = user.Username;
            return RedirectToAction("Index", "Home");
        }
    }
}