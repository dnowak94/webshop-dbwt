using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using System.Xml;
using System.Xml.Linq;
using webshop.Models.LINQ;

namespace webshop.Models
{
    public class XML
    {
        private const string datetimeFormat = "yyyy-mm-dd";
        public static List<XmlKategorie> Import(HttpPostedFileBase xmlFile)
        {
            var xmlRead = new StreamReader(xmlFile.InputStream);
            var document = XDocument.Load(xmlRead);
            var kategorien = document.Descendants("Kategorie");
            var resultList = new List<XmlKategorie>();
            var p = new PraktikumDataContext();
            // Kategorie
            foreach (var k in kategorien)
            {
                var bezeichnung = k.Attribute("Bezeichnung");
                // wenn keine Bezeichnung vorhanden => Kategorie nicht einfügen
                if (bezeichnung == null)
                    continue;
                var xmlKategorie = new XmlKategorie() { Bezeichnung = bezeichnung.Value };
                Debug.WriteLine("Kategorie : " + bezeichnung.Value);

                // Produkte
                var produkte = k.Descendants("Produkt");

                foreach (var prod in produkte)
                {
                    var name = prod.Attribute("Name").Value;
                    var von = prod.Attribute("von");
                    var bis = prod.Attribute("bis");
                    System.Diagnostics.Debug.WriteLine("Produkt: " + name);

                    var produktAttribute = prod.Descendants();
                    // erstes Beschreibung-Tag holen falls mehrere vorhanden
                    var beschreibung = produktAttribute.FirstOrDefault(x => x.Name.ToString().Equals("Beschreibung"));
                    var preise = produktAttribute.Where(x => x.Name.ToString().Equals("Preis"));

                    var xmlProdukt = new XmlProdukt()
                    {
                        Name = name,
                        Beschreibung = beschreibung != null ? beschreibung.Value : "",
                        von =
                            von != null
                                ? XmlConvert.ToDateTime(von.Value, XmlDateTimeSerializationMode.Local)
                                : SqlDateTime.MinValue.Value,
                        bis =
                            bis != null
                                ? XmlConvert.ToDateTime(bis.Value, XmlDateTimeSerializationMode.Local)
                                : SqlDateTime.MaxValue.Value
                    };

                    // Preise
                    foreach (var pr in preise)
                    {
                        var rolle = pr.Attribute("Gruppe").Value;
                        var betrag = pr.Attribute("Betrag").Value;

                        var preis_obj = new LINQ.Preis()
                        {
                            Preis1 = Convert.ToDecimal(betrag),
                            Rolle = rolle
                        };
                        xmlProdukt.preise.Add(preis_obj);
                        //System.Diagnostics.Debug.WriteLine("Preis (" + rolle + ";" + preis_obj.Preis1 + ")");
                    }
                    xmlKategorie.produkts.Add(xmlProdukt);
                }
                resultList.Add(xmlKategorie);
            }
            return resultList;
        }

        public static void Import2(HttpPostedFileBase xmlFile)
        {

            var xmlRead = new StreamReader(xmlFile.InputStream);
            var document = XDocument.Load(xmlRead);
            var kategorien = document.Descendants("Kategorie");
            var resultList = new List<XmlKategorie>();
            var p = new PraktikumDataContext();
            // Kategorie
            foreach (var k in kategorien)
            {
                var bezeichnung = k.Attribute("Bezeichnung");
                // wenn keine Bezeichnung vorhanden => Kategorie nicht einfügen
                if (bezeichnung == null)
                    continue;
                var kategorie = p.Kategorie.FirstOrDefault(x => x.Bezeichnung.Equals(bezeichnung.Value));
                if (kategorie == null)
                {
                    kategorie = new Kategorie() { Bezeichnung = bezeichnung.Value };
                    p.Kategorie.InsertOnSubmit(kategorie);
                    p.SubmitChanges();
                }

                int kat_id = kategorie.ID;

                // Produkte
                var produkte = k.Descendants("Produkt");

                foreach (var prod in produkte)
                {
                    var name = prod.Attribute("Name").Value;
                    var von = prod.Attribute("von");
                    var bis = prod.Attribute("bis");
                    System.Diagnostics.Debug.WriteLine("Produkt: " + name);

                    var produktAttribute = prod.Descendants();
                    // erstes Beschreibung-Tag holen falls mehrere vorhanden
                    var beschreibung = produktAttribute.FirstOrDefault(x => x.Name.ToString().Equals("Beschreibung"));
                    var preise = produktAttribute.Where(x => x.Name.ToString().Equals("Preis"));

                    var produkt = p.Produkt.FirstOrDefault(x => x.Name.Equals(name));
                    if (produkt == null)
                    {
                        produkt = new LINQ.Produkt()
                        {
                            Name = name,
                            Beschreibung = beschreibung != null ? beschreibung.Value : "",
                            von =
                                von != null
                                    ? XmlConvert.ToDateTime(von.Value, XmlDateTimeSerializationMode.Local)
                                    : SqlDateTime.MinValue.Value,
                            bis =
                                bis != null
                                    ? XmlConvert.ToDateTime(bis.Value, XmlDateTimeSerializationMode.Local)
                                    : SqlDateTime.MaxValue.Value,
                            FK_Kategorie = kat_id
                        };
                        p.Produkt.InsertOnSubmit(produkt);
                    }
                    else
                    {
                        produkt.Name = beschreibung != null ? beschreibung.Value : "";
                        produkt.von = von != null
                            ? XmlConvert.ToDateTime(von.Value, XmlDateTimeSerializationMode.Local)
                            : SqlDateTime.MinValue.Value;
                        produkt.bis = bis != null
                            ? XmlConvert.ToDateTime(bis.Value, XmlDateTimeSerializationMode.Local)
                            : SqlDateTime.MaxValue.Value;
                        produkt.FK_Kategorie = kat_id;
                    }
                    p.SubmitChanges();

                    // Preise
                    foreach (var pr in preise)
                    {
                        var rolle = pr.Attribute("Gruppe").Value;
                        var betrag = pr.Attribute("Betrag").Value;

                        var preis_obj = p.Preis.FirstOrDefault(x => x.FK_Produkt == produkt.ID && x.Rolle.Equals(rolle));
                        if (preis_obj == null)
                        {
                            preis_obj = new LINQ.Preis()
                            {
                                Preis1 = Convert.ToDecimal(betrag),
                                Rolle = rolle,
                                FK_Produkt = produkt.ID
                            };
                            p.Preis.InsertOnSubmit(preis_obj);
                        }
                        else
                        {
                            preis_obj.Preis1 = Convert.ToDecimal(betrag);
                        }
                        p.SubmitChanges();

                        //System.Diagnostics.Debug.WriteLine("Preis (" + rolle + ";" + preis_obj.Preis1 + ")");
                    }
                }
            }
        }
    }

    public class XmlKategorie : LINQ.Kategorie
    {
        public List<XmlProdukt> produkts = new List<XmlProdukt>();
    }

    public class XmlProdukt : LINQ.Produkt
    {
        public List<Preis> preise = new List<Preis>();
    }
}