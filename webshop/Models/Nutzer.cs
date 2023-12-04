using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Caching;
using webshop.Models.LINQ;

namespace webshop.Models
{
    public class Nutzer
    {
        [DisplayName("Nutzer")]
        public string Username { get; set; }
        public string Gruppe { get; set; }
        public DateTime Zeitpunkt { get; set; }

        public static IEnumerable<Nutzer> GetLastLoggedIn()
        {
            var p = new PraktikumDataContext();
            var defaultDate = DateTime.Parse("01.01.1900 00:00:00");
            var query = p.FE_Nutzer
                .Where(x => x.letzterLogin != defaultDate)
                .Take(5)
                .Select(x => new Nutzer()
            {
                Gruppe = x.FH_Angehörige.Student != null ? "Student" : "Mitarbeiter",
                Username = GetUsername(x.FH_Angehörige.E_Mail),
                Zeitpunkt = x.letzterLogin
            }).OrderByDescending(x => x.Zeitpunkt);
            return query;
        }

        private static string GetUsername(string email)
        {
            return email.Substring(0, email.IndexOf('.')).ToLower();
        }
    }
}