//using System.Data.SqlClient;
//using System.Configuration;
//namespace Milestone2_MVC.Models
//{
//    public class MyDB
//    {
//        #region properties

//        private static readonly string connStr = ConfigurationManager.ConnectionStrings["dbConnStr"].ConnectionString;
//        public SqlConnection conn { get; set; }
//        public SqlCommand sqlCmd { get; }
//        public string query { get; set; }
//        #endregion

//        #region Constructor

//        public MyDB(string query)
//        {
//            this.query = query;
//            conn = new SqlConnection(connStr);
//            sqlCmd = new SqlCommand(query,conn);
//        }

//        public SqlDataReader GetReader()
//        {
//            return sqlCmd.ExecuteReader();
//        }

//        public void Close()
//        {
//            conn.Close();
//            sqlCmd.Dispose();
//        }
//#endregion
//    }
//}