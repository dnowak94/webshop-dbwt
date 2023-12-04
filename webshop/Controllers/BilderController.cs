using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webshop.Models;
using webshop.Models.LINQ;

namespace webshop.Controllers
{
    public class BilderController : Controller
    {
        // GET: Bilder
        public ActionResult Index()
        {
            if(Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            IEnumerable<ViewBild> list = ViewBild.GetAll();
            if (list == null)
            {
                list = new List<ViewBild>();
            }
            return View(list);
        }

        // GET: Bilder/Details/5
        public ActionResult Details(int id)
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            return View();
        }

        // GET: Bilder/Create
        public ActionResult Create()
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            return View();
        }

        // POST: Bilder/Create
        [HttpPost]
        public ActionResult Create(webshop.Models.MyBild b)
        {
            if (ModelState.IsValid)
            {
                b.File = HttpContext.Request.Files["bild"];
                if (b.File != null && b.File.ContentLength == 0)
                {
                    ViewBag.ErrorMessage = "Bitte laden Sie ein Bild hoch.";
                    return View();
                }

                using (var p = new PraktikumDataContext())
                {
                    // insert image into db 
                    var imageInsert = new Models.LINQ.Bild
                    {
                        Title = b.Title,
                        Alt_Text = b.Alt_Text,
                        Unterschrift = b.Unterschrift,
                        Binärdaten = Base64.ConvertToBytes(b.File)
                    };
                    p.Bild.InsertOnSubmit(imageInsert);
                    p.SubmitChanges();
                }

                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Bilder/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["authenticatedBE"] == null || Session["authenticatedBE"].Equals(""))
                return RedirectToAction("Login", "Admin");
            var b = Models.MyBild.GetBild(id);
            if (b != null)
            {
                return View(b);
            }
            return RedirectToAction("Index");
        }

        // POST: Bilder/Edit/5
        [HttpPost]
        public ActionResult Edit(webshop.Models.MyBild b)
        {
            if (ModelState.IsValid)
            {
                b.File = HttpContext.Request.Files["bild"];
                using (var p = new PraktikumDataContext())
                {
                    // update image
                    var query = p.Bild.First(x => x.ID == b.Id);
                    // falls neue Datei hochgeladen
                    if (b.File != null)
                    {
                        query.Binärdaten = Base64.ConvertToBytes(b.File);
                    }
                    query.Title = b.Title;
                    query.Alt_Text = b.Alt_Text;
                    query.Unterschrift = b.Unterschrift;
                    
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
            var b = ViewBild.GetBild(id);
            if (b == null)
                return RedirectToAction("Index", "Bilder");
            return View(b);
        }

        // POST: Bilder/Delete/5
        [HttpPost]
        public ActionResult Delete(Models.LINQ.Bild b)
        {
            try
            {
                var p = new PraktikumDataContext();
                // alle Produkte suchen die auf das Bild referenzieren und löschen
                var produkte = p.Produkt.Where(x => x.FK_Bild == b.ID);
                foreach (var pr in produkte)
                {
                    pr.Bild = null;
                }
                p.SubmitChanges();

                p.Bild.DeleteOnSubmit(b);
                p.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch (NullReferenceException e)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
