using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace webshop.Controllers
{
    public class BenutzerController : Controller
    {
        // GET: Benutzer
        public ActionResult Index()
        {
            return View();
        }

        // GET: Benutzer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Benutzer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Benutzer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Benutzer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // PUT: Benutzer/Edit/5
        [HttpPut]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Benutzer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // DELETE: Benutzer/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
