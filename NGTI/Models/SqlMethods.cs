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
        public static List<Employee> GetUsers()
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM AspNetUsers";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Employee>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Employee();
                    obj.Id = (string)rdr["Id"];
                    obj.Email = (string)rdr["Email"];
                    obj.BHV = (bool)rdr["BHV"];
                    obj.Admin = (bool)rdr["Admin"];
                    model.Add(obj);
                }
            }
            conn.Close();
            return model;
        }
        public static List<Employee> GetUsersForEdit(string name)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM AspNetUsers u WHERE u.Id NOT IN (SELECT UserId FROM TeamMembers WHERE TeamName = '" + name + "')";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Employee>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Employee();
                    obj.Id = (string)rdr["Id"];
                    obj.Email = (string)rdr["Email"];
                    obj.BHV = (bool)rdr["BHV"];
                    obj.Admin = (bool)rdr["Admin"];
                    model.Add(obj);
                }
            }
            conn.Close();
            return model;
        }

    }
}
