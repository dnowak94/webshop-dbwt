using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using webshop.Models;
using webshop.Models.LINQ;

namespace webshop.Controllers
{
    public class DatenimportController : Controller
    {
        // GET: Datenimport
        public ActionResult Index()
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            HttpPostedFileBase xmlFile = HttpContext.Request.Files["xmlFile"]; // <input type="file" name="xmlFile" />

            if (xmlFile != null && xmlFile.ContentLength == 0)
            {
                ViewBag.ErrorMessage = "Bitte laden Sie eine Datei hoch.";
                return View(); // no upload in this input field?
            }

            var xmlImport = XML.Import(xmlFile);
            if (xmlImport == null)
            {
                ViewBag.ErrorMessage = "Import fehlgeschlagen.";
                return View();
            }
            return View("XMLImport",xmlImport);
        }

        [HttpPost]
        public ActionResult XMLImport(List<XmlKategorie> import)
        {
            var p = new PraktikumDataContext();
            foreach (var item in import)
            {
                // Kategorie vorhanden => neue hinzufügen
                var kategorie = p.Kategorie.FirstOrDefault(x => x.Bezeichnung.Equals(item.Bezeichnung));
                if (kategorie == null)
                {
                    kategorie = new Kategorie() {Bezeichnung = item.Bezeichnung};
                    p.Kategorie.InsertOnSubmit(kategorie);
                    p.SubmitChanges();
                }

                int kat_id = kategorie.ID;
                // FK_Kategorie in Produkte setzen
                foreach (var produkt in item.produkts)
                {
                    produkt.FK_Kategorie = kat_id;
                    p.Produkt.InsertOnSubmit(produkt);
                    p.SubmitChanges();
                    // FK_Preis in produkt setzen
                    foreach (var preis in produkt.preise)
                    {
                        preis.FK_Produkt = produkt.ID;
                        p.Preis.InsertOnSubmit(preis);
                        p.SubmitChanges();
                    }
                }
            }
            p.SubmitChanges();
            ViewBag.Success = "Daten erfolgreich importiert.";
            return View("Index");
        }

        public ActionResult XMLImport2()
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            return View();
        }

        [HttpPost]
        public ActionResult XMLImport2(FormCollection form)
        {
            HttpPostedFileBase xmlFile = HttpContext.Request.Files["xmlFile"]; // <input type="file" name="xmlFile" />

            if (xmlFile != null && xmlFile.ContentLength == 0)
            {
                ViewBag.ErrorMessage = "Bitte laden Sie eine Datei hoch.";
                return View(); // no upload in this input field?
            }

            XML.Import2(xmlFile);
            ViewBag.Success = "Daten erfolgreich importiert.";
            return View();
        }

    }
}