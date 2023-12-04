using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webshop.Models.LINQ;

namespace webshop.Models
{
    public class ViewBild : LINQ.Bild
    {
        /// <summary>
        /// avoid binary issues in view
        /// </summary>
        public string Base64Representation => Base64.ToBase64(this.Binärdaten.ToArray());

        /// <summary>
        /// gets all images from db and returns them in list of suitable model ViewBild
        /// </summary>
        /// <returns>all images from "Bild" table in ViewBild</returns>
        public static List<ViewBild> GetAll()
        {
            List<ViewBild> list = null;
            using (var p = new PraktikumDataContext())
            {
                list = p.Bild.Select(b => new ViewBild
                {
                    ID = b.ID,
                    Binärdaten = b.Binärdaten,
                    Alt_Text = b.Alt_Text,
                    Title = b.Title,
                    Unterschrift = b.Unterschrift
                }).ToList();
            }
            return list;
        }

        public static LINQ.Bild GetBild(int id)
        {
            LINQ.Bild b = null;
            using (var p = new PraktikumDataContext())
            {
                b = p.Bild.FirstOrDefault(x => x.ID == id);
            }
            return b;
        }

        public ViewBild() : base()
        {

        }

        public ViewBild(LINQ.Bild bild)
        {
            ID = bild.ID;
            Title = bild.Title;
            Alt_Text = bild.Alt_Text;
            Binärdaten = bild.Binärdaten;
            Unterschrift = bild.Unterschrift;
            Kategorie = bild.Kategorie;
            Produkt = bild.Produkt;
        }
    }
}