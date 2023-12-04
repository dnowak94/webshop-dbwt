using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webshop.Models;
using PasswordSecurity;

namespace webshop.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login l)
        {
            if (ModelState.IsValid)
            {
                if (l.LoginCheck(l.Benutzername, l.Passwort))
                {
                    Session["authenticated"] = l.Id;
                    Session["role"] = l.Rolle;
                    ModelState.Clear();
                    ViewBag.Success = "Login erfolgreich.";
                }
                else
                {
                    Session["authenticated"] = "";
                    ViewBag.ErrorMessage = "Login nicht erfolgreich.";
                    ViewBag.ErrorDescription = "falsche Kombination von Nutzername und Kennwort.";
                }
            }
            return View();
        }

    }
}