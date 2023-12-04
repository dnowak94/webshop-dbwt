using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.WebPages;
using webshop.Models.LINQ;

namespace webshop.Models
{
    public class Produkt : LINQ.Produkt
    {
        public decimal price { get; set; }

        public Produkt(LINQ.Produkt produkt)
        {
            ID = produkt.ID;
            Name = produkt.Name;
            Beschreibung = produkt.Beschreibung;
            Bild = produkt.Bild;
            von = produkt.von;
            bis = produkt.bis;
            Kategorie = produkt.Kategorie;
            FK_Bild = produkt.FK_Bild;
            FK_Kategorie = produkt.FK_Kategorie;
            Kauft = produkt.Kauft;
            Schreibt = produkt.Schreibt;
            hatZutat = produkt.hatZutat;
        }

        private static List<LINQ.Produkt> GetProdukts()
        {
            var p = new PraktikumDataContext();
            var query = p.Produkt
                .Where(x => x.von <= DateTime.Now)
                .Where(x => x.bis >= DateTime.Now).ToList();
            return query;
        }

        public static Produkt GetProduktDetails(int id, string role = "")
        {
            var p = GetProdukts().FirstOrDefault(x => x.ID == id);
            Produkt res = null;
            if (p != null)
            {
                res = new Produkt(p);
                res.price = GetPreis(id, role);
            }
            return res;
        }

        public static decimal GetPreis(int produkt_id, string role = "")
        {
            var p = new PraktikumDataContext();
            var query = p.Preis
                        .Where(x => x.FK_Produkt == produkt_id && x.Rolle.Equals(!string.IsNullOrEmpty(role) ? role : "Gast"))
                        .Select(x => new { x.Preis1 }).FirstOrDefault();
            if (query != null)
            {
                return query.Preis1;
            }
            return -1;
        }

        public static List<LINQ.Produkt> GetByKategorie(string kategorie = "")
        {
            var query = GetProdukts();
            if (!string.IsNullOrEmpty(kategorie))
            {
                query.Where(x => x.Kategorie.Bezeichnung.Equals(kategorie));
            }
            return query;
        }

        public static bool IsValidId(int id)
        {
            var p = new PraktikumDataContext();
            var query = p.Produkt.FirstOrDefault(x => x.ID == id);
            return (query != null);
        }

        public static List<Link> GetLayoutLinks()
        {
            var p = new PraktikumDataContext();
            var query = p.Produkt
                        .Where(x => x.von <= DateTime.Now)
                        .Where(x => x.bis >= DateTime.Now)
                        .Take(4)
                        .Select(x => new Link() { Id = x.ID, Name = x.Name }).ToList();
            return query;
        }

        public static SelectList GetDropdownList(int id = 0)
        {
            var produktliste = GetProdukts().OrderBy(x => x.Name);
            var selectlist = new List<SelectListItem>();
            foreach (var p in produktliste)
            {
                selectlist.Add(new SelectListItem() { Text = p.Name, Value = p.ID.ToString() });
            }
            var query = selectlist.FirstOrDefault(x => x.Value.AsInt() == id);
            if (query != null)
            {
                query.Selected = true;
            }
            else
            {
                selectlist[0].Selected = true;
            }
            return new SelectList(selectlist, "Value", "Text");
        }
    }

    public class Link
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}