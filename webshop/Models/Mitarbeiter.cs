using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace webshop.Models
{
    public class Mitarbeiter : FH_Angehörige
    {
        #region properties
        public string Büro { get; set; }
        #endregion

        public void Registrieren()
        {
            base.Registrieren();
            // Student einfügen
            var query = "INSERT INTO [Mitarbeiter] (FK_FH_Angehörige, Büro) VALUES(" +
                        Id + ", '" + Büro + "')";
            var sqlCmd = new SqlCommand(query, Db.sqlcon);
            sqlCmd.ExecuteNonQuery();
        }
    }
}