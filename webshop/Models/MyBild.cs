using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using webshop.Models.LINQ;

namespace webshop.Models
{
    public class MyBild
    {
        #region properties
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Geben Sie einen Titel an.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Geben Sie einen AltText an.")]
        public string Alt_Text { get; set; }
        public string Unterschrift { get; set; }
        public byte[] Binärdaten { get; set; }

        public HttpPostedFileBase File { get; set; }
        #endregion

        public string Base64Representation => Base64.ToBase64(this.Binärdaten.ToArray());

        public static bool IsValid(int id)
        {
            var p = new PraktikumDataContext();
            var query = p.Bild.FirstOrDefault(x => x.ID == id);
            if (query != null)
                return true;
            return false;
        }
        public static MyBild GetBild(int id)
        {
            if (IsValid(id))
            {
                var p = new PraktikumDataContext();
                var query = p.Bild.Where(x => x.ID == id)
                    .Select(x => new MyBild()
                    {
                        Id = x.ID,
                        Title = x.Title,
                        Alt_Text = x.Alt_Text,
                        Unterschrift = x.Unterschrift,
                        Binärdaten = x.Binärdaten.ToArray()
                    }).FirstOrDefault();
                return query;
            }
            System.Diagnostics.Debug.WriteLine("id is not valid!");
            return null;
        }

        public static List<LINQ.Bild> GetList()
        {
            List<LINQ.Bild> list = null;
            using (var p = new PraktikumDataContext())
            {
                list = p.Bild.ToList();
            }
            return list;
        }

        public static SelectList GetDropdownList(int id)
        {
            var bildliste = MyBild.GetList().OrderBy(x => x.Title);
            var selectlist = new List<SelectListItem>();
            selectlist.Add(new SelectListItem() {Text = "", Value = "0"});
            foreach (var b in bildliste)
            {
                selectlist.Add(new SelectListItem() { Text = b.Title, Value = b.ID.ToString() });
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
}