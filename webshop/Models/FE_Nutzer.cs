using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PasswordSecurity;

namespace webshop.Models
{
    public class FE_Nutzer
    {
        #region properties
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie ein Passwort ein!")]
        [DisplayName("Passwort")]
        [DataType(DataType.Password)]
        public string Passwort1 { get; set; }
        [Compare("Passwort1",ErrorMessage = "Bitte bestätigen Sie ihr Passwort.")]
        [DisplayName("Passwort wiederholen")]
        [DataType(DataType.Password)]
        public string Passwort2 { get; set; }
        #endregion

        public void Registrieren()
        {
            if (!Db.AssertConnected())
                return;
            // FE-Nutzer
            var hash = PasswordStorage.CreateHash(Passwort1).Split(':');
           var query = "INSERT INTO [FE-Nutzer] (FK_BE_Nutzer,Stretch,Algo,Salt,Hash) VALUES(NULL," +
                    PasswordStorage.PBKDF2_ITERATIONS + ", '" + hash[PasswordStorage.HASH_ALGORITHM_INDEX] + "', '" +
                    hash[PasswordStorage.SALT_INDEX] + "', '" + hash[PasswordStorage.PBKDF2_INDEX] + "')";
            System.Diagnostics.Debug.WriteLine(query);
            var sqlCmd = new SqlCommand(query, Db.sqlcon);
            sqlCmd.ExecuteNonQuery();
            sqlCmd.CommandText = "SELECT TOP 1 ID FROM [FE-Nutzer] ORDER BY ID DESC";
            var reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                Id = (int) reader["ID"];
            }
            reader.Close();
        }

        public static bool BereitsVorhanden(string email,string name="")
        {
            if (!Db.AssertConnected())
                return true; // SqlConnection fail => register fail
            
            // FH-Angehörige
            var query = "SELECT FK_FE_Nutzer FROM [FH-Angehörige] WHERE E_Mail='" + email + "'";
            SqlCommand sqlCmd = new SqlCommand(query, Db.sqlcon);
            var reader = sqlCmd.ExecuteReader();
            bool res = reader.Read();
            reader.Close();
            if (res)
                return true;
            if (!string.IsNullOrEmpty(name))
            {
                query = "SELECT FK_FE_Nutzer FROM [Gast] WHERE Name='" + name + "'";
                sqlCmd = new SqlCommand(query,Db.sqlcon);
                reader = sqlCmd.ExecuteReader();
                res = reader.Read();
                reader.Close();
            }
            return res;
        }
    }
}