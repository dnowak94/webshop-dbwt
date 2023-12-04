using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PasswordSecurity;
using webshop.Models.LINQ;

namespace webshop.Models
{
    public class Login
    {
        #region properties

        public int Id { get; set; }
        [Required(ErrorMessage = "Geben Sie den Benutzernamen ein.")]
        public string Benutzername { get; set; }
        [Required(ErrorMessage = "Passwort darf nicht leer sein.")]
        [DataType(DataType.Password)]
        public string Passwort { get; set; }
        public string Rolle { get; set; }
        #endregion


        public bool LoginCheck(string username, string passwort)
        {
            if (!Db.AssertConnected())
                return false;
            var query =
                "SELECT ID,Stretch,Algo,Salt,Hash FROM [FE-Nutzer] JOIN [FH-Angehörige] AS FH ON ID=FH.FK_FE_Nutzer WHERE Aktiv=1 AND FH.E_Mail='" +
                username + "'";
            System.Diagnostics.Debug.WriteLine(query);
            var sqlCmd = new SqlCommand(query, Db.sqlcon);
            var reader = sqlCmd.ExecuteReader();
            // Username korrekt?
            if (reader.Read())
            {
                var hash = reader["Algo"] + ":" + reader["Stretch"] + ":" + PasswordStorage.HASH_BYTES + ":" +
                           reader["Salt"] + ":" + reader["Hash"];
                if (PasswordStorage.VerifyPassword(passwort, hash))
                {
                    Id = (int)reader["ID"];

                    // letzterLogin eintragen
                    var p = new PraktikumDataContext();
                    p.FE_Nutzer.Where(x => x.ID == Id).First().letzterLogin = DateTime.Now;
                    p.SubmitChanges();

                    // Student?
                    var query2 = p.Student.Where(x => x.FK_FH_Angehörige == Id).FirstOrDefault();
                    if (query2 != null)
                    {
                        Rolle = "Student";
                    }
                    else
                    {
                        // Mitarbeiter ?
                        var query3 = p.Mitarbeiter.Where(x => x.FK_FH_Angehörige == Id).FirstOrDefault();
                        if (query3 != null)
                        {
                            Rolle = "Mitarbeiter";
                        }
                    }
                    reader.Close();
                    return true;
                }
            }
            reader.Close();
            return false;
        }

        public bool LoginCheckBE()
        {
            var p = new PraktikumDataContext();
            var query = p.BE_Nutzer.Where(x => x.E_Mail.Equals(Benutzername)).FirstOrDefault();
            if (query == null)
                return false;
            var hash = query.Algo + ":" + query.Stretch + ":" + PasswordStorage.HASH_BYTES + ":" +
                       query.Salt + ":" + query.Hash;
            if (PasswordStorage.VerifyPassword(Passwort, hash))
            {
                query.letzterLogin = DateTime.Now;
                p.SubmitChanges();
                Id = query.ID;
                return true;
            }
            return false;
        }

        public static Login Load(int id, string role)
        {
            if (!Db.AssertConnected())
                return null;
            Login res = null;
            var query = "SELECT Name FROM [" + (!role.Equals("Gast") ? "FH-Angehörige" : "Gast") +
                        "] WHERE FK_FE_Nutzer=" + id;
            var sqlCmd = new SqlCommand(query, Db.sqlcon);
            var reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                res = new Login
                {
                    Id = id,
                    Benutzername = reader["Name"].ToString(),
                    Rolle = role
                };
            }
            reader.Close();
            return res;
        }
    }
}