using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class GroupReservationDBAccesLayer
    {
        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true");
        public string AddGroupReservationRecord(GroupReservation GroupReservationEntities)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("AddNewGroupResDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdGroupReservation", GroupReservationEntities.IdGroupReservation);
                cmd.Parameters.AddWithValue("@Name", GroupReservationEntities.Name);
                cmd.Parameters.AddWithValue("@TeamName", GroupReservationEntities.Teamname);
                cmd.Parameters.AddWithValue("@Date", GroupReservationEntities.Date);
                cmd.Parameters.AddWithValue("@StartTime", GroupReservationEntities.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", GroupReservationEntities.EndTime);
                cmd.Parameters.AddWithValue("@Reason", GroupReservationEntities.Reason);
                cmd.Parameters.AddWithValue("@TableId", GroupReservationEntities.TableId);
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
