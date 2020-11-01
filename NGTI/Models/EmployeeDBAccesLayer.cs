
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace NGTI.Models
{
    public class EmployeeDBAccessLayer
    {
        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true");
        public string AddEmployeeRecord(Employee employeeEntities)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("AddNewEmpdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", employeeEntities.Email);
                cmd.Parameters.AddWithValue("@Name", employeeEntities.Name);
                cmd.Parameters.AddWithValue("@BHV", employeeEntities.BHV);
                cmd.Parameters.AddWithValue("@Admin", employeeEntities.Admin);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Data save Successfully");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return (ex.Message.ToString());
            }
        }
    }
}
