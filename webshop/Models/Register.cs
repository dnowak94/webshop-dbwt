//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.WebPages;
//using PasswordSecurity;

//namespace Milestone2_MVC.Models
//{
//    public class Register
//    {
//        #region properties
//        public bool Email_failed { get; set; }
//        public bool Passwort_failed { get; set; }
//        public bool Fachbereich_failed { get; set; }
//        public bool Studiengang_failed { get; set; }
//        public bool BereitsVorhanden { get; set; }
//        public bool Register_failed { get; set; }
//        #endregion
//        private static int FK_FE_Nutzer { get; set; }

//        public static Register RegisterCheck(FormCollection form)
//        {
//            Register res = new Register
//            {
//                Passwort_failed = form["passwort1"].Equals(form["passwort2"]),
//                Email_failed = form["email1"].Equals(form["email2"]),
//                Fachbereich_failed = string.IsNullOrEmpty(form["fachbereich"].ToString()),
//                Studiengang_failed = ((form["rolle"].Equals("student") && !string.IsNullOrWhiteSpace(form["studiengang"]))
//            };
//            res.Register_failed = (res.Passwort_failed | res.Email_failed | res.Fachbereich_failed |
//                                   res.Studiengang_failed);
//            res.BereitsVorhanden = (res.Register_failed |
//                                    Registrieren(form["name"], form["email1"], form["passwort1"], form["fachbereich"].AsInt()));
//            res.Register_failed = res.BereitsVorhanden;

//            // Register nicht failed?
//            if (!res.Register_failed)
//            {
//                // Student registrieren?
//                if (form["rolle"].Equals("Student"))
//                {
//                    RegisterStudent(form["studiengang"]);
//                }
//                else
//                {
//                    if (form["rolle"].Equals("Mitarbeiter"))
//                    {
//                        RegisterMitarbeiter(form["büro"]);
//                    }
//                }
//            }
//            return res;
//        }

//        private static bool Registrieren(string name, string email, string passwort, int fachbereich)
//        {
//            var conStr = ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString;
//            using (SqlConnection con = new SqlConnection())
//            {
//                // Benutzer vorhanden?
                
                

//                // FE-Nutzer ID holen

                

                

//                con.Close();
//            }
//            return true;
//        }

//        private static void RegisterStudent(string studiengang)
//        {
//            var conStr = ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString;
//            using (SqlConnection con = new SqlConnection())
//            {
                
//            }
//        }

//        private static void RegisterMitarbeiter(string büro="")
//        {
//            var conStr = ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString;
//            using (SqlConnection con = new SqlConnection())
//            {
//                // Mitarbeiter einfügen
//                var query = "INSERT INTO [Mitarbeiter] (FK_FH_Angehörige, Büro) VALUES(" +
//                            FK_FE_Nutzer + ", '" + büro + "')";
//                var sqlCmd = new SqlCommand(query, con);
//                sqlCmd.ExecuteNonQuery();
//                sqlCmd.Dispose();
//            }
//        }
//    }
//}
