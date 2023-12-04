using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class ProduktBild
    {
        public Produkt Produkt { get; set; }
        public MyBild Bild { get; set; }
    }
}