using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webshop.Models;
using webshop.Models.LINQ;

namespace webshop.Controllers
{
    public class ProdukteController : Controller
    {
        // GET: Produkte
        public ActionResult Index()
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login","Admin");
            var liste = Models.Produkt.GetByKategorie();
            if (liste == null)
            {
                liste = new List<Models.LINQ.Produkt>();
            }
            return View(liste);
        }

        // GET: Produkte/Create
        public ActionResult Create()
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            return View();
        }

        // POST: Produkte/Create
        [HttpPost]
        public ActionResult Create(Models.LINQ.Produkt pr)
        {
            if (ModelState.IsValid)
            {
                using (var p = new PraktikumDataContext())
                {
                    pr.FK_Kategorie = 1;
                    // insert produkt into db
                    p.Produkt.InsertOnSubmit(pr);
                    p.SubmitChanges();
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Produkte/Edit/5
        public ActionResult Edit(int id=0)
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            string role = Session["role"] != null && string.IsNullOrEmpty(Session["role"].ToString())
                ? Session["role"].ToString()
                : "Gast";
            var p = new PraktikumDataContext();
            var produkt = p.Produkt.FirstOrDefault(x => x.ID == id);
            if (p != null)
            {
                return View(produkt);
            }
            return RedirectToAction("Index", "Produkte");
        }

        // POST: Produkte/Edit/5
        [HttpPost]
        public ActionResult Edit(Models.LINQ.Produkt pr)
        {
            if (ModelState.IsValid)
            {
                using (var p = new PraktikumDataContext())
                {
                    var query = p.Produkt.FirstOrDefault(x => x.ID == pr.ID);
                    query.Name = pr.Name;
                    query.Beschreibung = pr.Beschreibung;
                    if (pr.FK_Bild != null)
                    {
                        query.FK_Bild = pr.FK_Bild;
                        query.Bild = p.Bild.FirstOrDefault(x => x.ID == pr.FK_Bild.Value);
                    }
                    p.SubmitChanges();
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Bilder/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            var p = Models.Produkt.GetProduktDetails(id);
            if (p == null)
                return RedirectToAction("Index", "Produkte");
            return View(p);
        }

        // POST: Bilder/Delete/5
        [HttpPost]
        public ActionResult Delete(Models.LINQ.Produkt pr)
        {
            var p = new PraktikumDataContext();
            p.Produkt.DeleteOnSubmit(pr);
            p.SubmitChanges();
            return RedirectToAction("Index","Produkte");
        }
    }
}
