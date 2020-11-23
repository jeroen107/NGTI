using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class SqlMethods
    {

        public static void QueryVoid(string query)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";

            SqlConnection conn = new SqlConnection(connectionString);
            string sql = query;
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
            }
            conn.Close();
        }
        public static int QueryLimit() 
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
            int limit = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT limit FROM Limit";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    limit = (int)rdr["limit"];
                }
            }
            conn.Close();
            return limit;
        }
    }
}
