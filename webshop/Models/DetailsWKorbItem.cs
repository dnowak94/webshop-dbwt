using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class DetailsWKorbItem
    {
        public Produkt p { get; set; }
        public WKorb.WKorbItem w { get; set; }
    }
}