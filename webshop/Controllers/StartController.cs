using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using webshop.Models;
using webshop.Models.LINQ;
using Produkt = webshop.Models.Produkt;

namespace webshop.Controllers
{
    public class StartController : Controller
    {
        // GET: Start
        public ActionResult Index()
        {
            return View();
        }

        // GET: Details
        public ActionResult Details(int id = 0)
        {
            if (id == 0 || !Produkt.IsValidId(id))
            {
                return RedirectToAction("Produkte");
            }
            var p = Produkt.GetProduktDetails(id, (Session["role"] != null ? Session["role"].ToString() : ""));
            return View(p);
        }

        // GET: Produkte
        public ActionResult Produkte(string kategorieName = "")
        {
            var liste = Produkt.GetByKategorie(kategorieName);
            return View(liste);
        }

        private List<WKorb.WKorbItem> ReadFromCookie()
        {
            var cname = "wkorb";
            var cookie = HttpContext.Request.Cookies.Get(cname);
            List<WKorb.WKorbItem> wkorb = null;
            if (cookie != null)
            {
                var json = cookie.Value;
                // Liste nicht leer?
                if (!string.IsNullOrEmpty(json) && !json.Equals("[]"))
                {
                    wkorb = System.Web.Helpers.Json.Decode<List<WKorb.WKorbItem>>(json);
                }
            }
            return wkorb;
        }

        private void WriteToCookie(List<WKorb.WKorbItem> wkorb)
        {
            var cname = "wkorb";
            var cookie = HttpContext.Request.Cookies.Get(cname);
            // cookie vorhanden?
            if (cookie == null)
            {
                cookie = new HttpCookie(cname);
            }
            cookie.Value = System.Web.Helpers.Json.Encode(wkorb);
            cookie.Expires = DateTime.Now.AddHours(1);
            HttpContext.Response.SetCookie(cookie);
        }

        public ActionResult Wkorb()
        {
            var wkorb = ReadFromCookie();
            // wenn cookie nicht vorhanden
            if (wkorb == null)
            {
                wkorb = new List<WKorb.WKorbItem>();
            }
            return View(wkorb);
        }

        [HttpPost]
        public ActionResult Details(FormCollection form)
        {
            if (Session["authenticated"] == null || Session["authenticated"].Equals(""))
                return RedirectToAction("Index", "Login");
            var p = new WKorb.WKorbItem(form["produkt_id"].AsInt(), form["anzahl"].AsInt(), (Session["role"] != null ? Session["role"].ToString() : ""));
            var wkorb = ReadFromCookie();
            if (wkorb == null)
            {
                wkorb = new List<WKorb.WKorbItem>();
            }
            WKorb WKorbWrapper = new WKorb(wkorb, Session["role"] != null ? Session["role"].ToString() : "");
            WKorbWrapper.AddToCart(p);
            WriteToCookie(WKorbWrapper.warenkorb);
            return RedirectToAction("Produkte");
        }

        public ActionResult Impressum()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(List<WKorb.WKorbItem> wkorb)
        {
            // wenn nicht eingeloggt zuerst Login
            if (Session["authenticated"] == null || Session["authenticated"].Equals(""))
                return RedirectToAction("Index", "Login");
            var p = new PraktikumDataContext();
            var warenkorb = new WKorb(wkorb, Session["role"] != null ? Session["role"].ToString() : "");
            var zahlung = new Zahlung()
            {
                Aut_Server = "127.0.0.0.1",
                Betrag = warenkorb.GetSumme(),
                Status = 10,
                Zeitstempel = DateTime.Now
            };
            p.Zahlung.InsertOnSubmit(zahlung);
            p.SubmitChanges();

            foreach (var w in wkorb)
            {
                var kauft = new Kauft()
                {
                    Endpreis = w.Preis(warenkorb.Rolle),
                    FK_FE_Nutzer = Session["authenticated"].ToString().AsInt(),
                    FK_Produkt = w.ProduktId,
                    FK_Zahlung = zahlung.ID
                };
                p.Kauft.InsertOnSubmit(kauft);
            }
            p.SubmitChanges();
            return View("Index");
        }
    }
}