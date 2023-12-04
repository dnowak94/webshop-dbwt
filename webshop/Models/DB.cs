using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;

namespace webshop.Models
{
    public static class Db
    {
        public static SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["PraktikumConnectionString"].ConnectionString);

        /// <summary>
        /// handles the connection of the sqlConnection
        /// </summary>
        /// <returns></returns>
        public static bool AssertConnected()
        {
            if (sqlcon.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["PraktikumConnectionString"].ConnectionString);
                sqlcon.Open();
                return sqlcon.State == ConnectionState.Open;
            }
        }
    }
}