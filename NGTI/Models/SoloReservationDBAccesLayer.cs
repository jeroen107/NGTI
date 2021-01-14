﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace NGTI.Models
{
    public class SoloReservationDBAccesLayer
    {
        SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true");
        public string AddSoloReservationRecord(SoloReservation SoloReservationEntities)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("AddNewSoloResDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdSoloReservation", SoloReservationEntities.IdSoloReservation);
                cmd.Parameters.AddWithValue("@Name", SoloReservationEntities.Name);
                cmd.Parameters.AddWithValue("@Date", SoloReservationEntities.Date);
                cmd.Parameters.AddWithValue("@TimeSlot", SoloReservationEntities.TimeSlot);
                cmd.Parameters.AddWithValue("@Reason", SoloReservationEntities.Reason);
                cmd.Parameters.AddWithValue("@Seat", SoloReservationEntities.Seat); 
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