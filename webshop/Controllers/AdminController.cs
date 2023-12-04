using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using webshop.Models;

namespace webshop.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login");
            var list = Nutzer.GetLastLoggedIn();
            return View(list);
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login l)
        {
            if (ModelState.IsValid)
            {
                if (l.LoginCheckBE())
                {
                    Session["authenticatedBE"] = l.Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["authenticatedBE"] = "";
                    ViewBag.ErrorMessage = "Login nicht erfolgreich.";
                    ViewBag.ErrorDescription = "falsche Kombination von Nutzername und Kennwort.";
                }
            }
            return View();
        }

        // GET: Register
        public ActionResult RegisterStudent()
        {
            return View();
        }

        public ActionResult RegisterMitarbeiter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterStudent(Student s)
        {
            if (ModelState.IsValid)
            {
                if (!FE_Nutzer.BereitsVorhanden(s.E_Mail1))
                {
                    s.Registrieren();
                    ModelState.Clear();
                    ViewBag.Success = s.Name + "wurde erfolgreich registriert.";
                }
                else
                {
                    ViewBag.ErrorMessage = "Registrieren fehlgeschlagen.";
                    ViewBag.ErrorDescription = "E-Mail bereits vorhanden.";
                }
            }
            return View();
        }


        [HttpPost]
        public ActionResult RegisterMitarbeiter(Mitarbeiter m)
        {
            if (ModelState.IsValid)
            {
                if (!FE_Nutzer.BereitsVorhanden(m.E_Mail1))
                {
                    m.Registrieren();
                    ModelState.Clear();
                    ViewBag.Success = m.Name + "wurde erfolgreich registriert.";
                }
                else
                {
                    ViewBag.ErrorMessage = "Registrieren fehlgeschlagen.";
                    ViewBag.ErrorDescription = "E-Mail bereits vorhanden.";
                }
            }
            return View();
        }
    }
}