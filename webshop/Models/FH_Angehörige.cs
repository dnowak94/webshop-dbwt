using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace webshop.Models
{
    public class FH_Angehörige : FE_Nutzer
    {
        #region properties
        [Required(ErrorMessage = "Bitte geben Sie einen Namen ein.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bitte geben Sie die E-Mail ein.")]
        [DisplayName("E-Mail")]
        public string E_Mail1 { get; set; }

        [Compare("E_Mail1",ErrorMessage = "Bitte bestätigen Sie die E-Mail.")]
        [DisplayName("E-Mail wiederholen")]
        public string E_Mail2 { get; set; }
        
        [Required(ErrorMessage = "Bitte geben Sie den Fachbereich an.")]        
        [Range(1,10,ErrorMessage = "Fachbereich muss zwischen 1 ... 10 sein.")]
        public int Fachbereich { get; set; }
        #endregion

        public void Registrieren()
        {
            base.Registrieren();
            // FH-Angehörige
            var query = "INSERT INTO [FH-Angehörige] (FK_FE_Nutzer,Name,E_Mail,Fachbereich) VALUES(" + Id + ", '" +
                    Name + "', '" + E_Mail1 + "', " + Fachbereich + ")";
            System.Diagnostics.Debug.WriteLine(query);
            var sqlCmd = new SqlCommand(query, Db.sqlcon);
            sqlCmd.ExecuteNonQuery();
            
            

            sqlCmd.CommandText = "SELECT TOP 1 FK_FE_Nutzer FROM [FH-Angehörige] ORDER BY FK_FE_Nutzer DESC";
            var reader = sqlCmd.ExecuteReader();
            if (reader.Read())
            {
                Id = (int) reader["FK_FE_Nutzer"];
            }
            reader.Close();
        }

        
    }
}