using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Web;
using System.Web.DynamicData;
using System.Web.Script.Serialization;

namespace webshop.Models
{
    public class WKorb
    {
        public string Rolle { get; set; }
        public List<WKorbItem> warenkorb { get; set; }

        public WKorb()
        {
            var cname = "wkorb";
            var cookie = HttpContext.Current.Request.Cookies.Get(cname);
            if (cookie != null)
            {
                var json = cookie.Value;
                if (!string.IsNullOrEmpty(json) && !json.Equals("[]"))
                {
                    warenkorb = System.Web.Helpers.Json.Decode<List<WKorbItem>>(json);
                }
            }
            else
            {
                warenkorb = new List<WKorbItem>();
            }
        }

        public WKorb(List<WKorbItem> warenkorb,string role)
        {
            this.warenkorb = warenkorb;
            this.Rolle = role;
        }

        public void AddToCart(WKorbItem produkt)
        {
            if (!warenkorb.Contains(produkt))
            {
                warenkorb.Add(produkt);
            }
            else
            {
                var item = warenkorb.First(x => x.ProduktId == produkt.ProduktId);
                item.Anzahl += produkt.Anzahl;
            }
        }

        public bool IstVorhanden()
        {
            return HttpContext.Current.Request.Cookies.Get("wkorb") != null;
        }

        public int GetAnzahlElemente()
        {
            return warenkorb.Sum(x => x.Anzahl);
        }

        public decimal GetSumme()
        {
            return warenkorb.Sum(x => x.Preis(Rolle));
        }


        [Serializable]
        public class WKorbItem
        {
            public int ProduktId { get; set; }
            public string ProduktName { get; set; }

            [Required(ErrorMessage = "Anzahl muss angegeben werden!")]
            [Range(1, int.MaxValue)]
            public int Anzahl { get; set; }

            public decimal Preis(string role)
            {
                return _unitPrice(role)*Anzahl;
            }

            private decimal _unitPrice(string role)
            {
                return Produkt.GetPreis(ProduktId, role);
            }

            public WKorbItem(int ProduktId, int Anzahl, string role)
            {
                this.ProduktId = ProduktId;
                this.Anzahl = Anzahl;
                var p = Produkt.GetProduktDetails(ProduktId, role);
                this.ProduktName = p.Name;
            }

            public WKorbItem()
            {
                ProduktId = -1;
                ProduktName = "";
                Anzahl = 0;
            }

            public bool Equals(WKorbItem other)
            {
                if (other == null)
                    return false;
                return this.ProduktId == other.ProduktId;
            }

            public override bool Equals(Object obj)
            {
                if (obj == null)
                    return false;
                return Equals(obj as WKorbItem);
            }
        }
    }
}