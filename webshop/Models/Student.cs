using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PasswordSecurity;

namespace webshop.Models
{
    public class Student : FH_Angehörige
    {
        #region properties

        [Required(ErrorMessage = "Bitte geben Sie den Studiengang an!")]
        public string Studiengang { get; set; }

        #endregion

        public void Registrieren()
        {
            base.Registrieren();
            using (
                SqlConnection con =
                    new SqlConnection(ConfigurationManager.ConnectionStrings["PraktikumConnectionString"].ConnectionString))
            {
                con.Open();
                // Student einfügen
                var query = "INSERT INTO [Student] (FK_FH_Angehörige, Studiengang) VALUES(" +
                            Id + ", '" + Studiengang + "')";
                System.Diagnostics.Debug.WriteLine(query);
                var sqlCmd = new SqlCommand(query, Db.sqlcon);
                sqlCmd.ExecuteNonQuery();
                sqlCmd.Dispose();
                con.Close();
            }
        }
    }
}
