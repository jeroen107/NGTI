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
            System.Diagnostics.Debug.WriteLine($"query = {query}");
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
        //sql list of groupreservations filter with sql string.
        public static List<GroupReservation> getGroupReservations(string sql)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<GroupReservation>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var groupRes = new GroupReservation();
                    groupRes.IdGroupReservation = (int)rdr["IdGroupReservation"];
                    groupRes.Name = (string)rdr["Name"];
                    groupRes.Teamname = (string)rdr["Teamname"];
                    groupRes.Date = (DateTime)rdr["Date"];
                    groupRes.TimeSlot = (string)rdr["TimeSlot"];
                    groupRes.Reason = (string)rdr["Reason"];
                    groupRes.Seat = (string)rdr["Seat"];
                    model.Add(groupRes);
                }
            }
            conn.Close();
            return(model);
        }
        public static GroupReservation getGroupReservation(string sql)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new GroupReservation();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var res = new GroupReservation();
                    res.IdGroupReservation = (int)rdr["IdGroupReservation"];
                    res.Teamname = (string)rdr["Teamname"];
                    res.Name = (string)rdr["Name"];
                    res.Date = (DateTime)rdr["Date"];
                    res.TimeSlot = (string)rdr["TimeSlot"];
                    res.Reason = (string)rdr["Reason"];
                    res.Seat = (string)rdr["Seat"];
                    model = res;
                }
            }
            conn.Close();
            return model;
        }
        //get list of soloreservations filter with sql string.
        public static List<SoloReservation> getSoloReservations(string sql)
        {
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<SoloReservation>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var soloRes = new SoloReservation();
                    soloRes.IdSoloReservation = (int)rdr["IdSoloReservation"];
                    soloRes.Name = (string)rdr["Name"];
                    soloRes.Date = (DateTime)rdr["Date"];
                    soloRes.TimeSlot = (string)rdr["TimeSlot"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.Seat = (string)rdr["Seat"];
                    model.Add(soloRes);
                }
            }
            conn.Close();
            return (model);
        }

    }
}
