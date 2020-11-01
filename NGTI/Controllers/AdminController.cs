using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGTI.Controllers
{
    public class AdminController : Controller
    {
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reservations()
        {
            //var reservationList = new List<SoloReservation>()
            //{
            //    new SoloReservation() { IdSoloReservation = 1, Name = "john", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,15,0,0), Reason = "", TableId = 1, Table = null},
            //    new SoloReservation() { IdSoloReservation = 2, Name = "sponge", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,20,0,0), Reason = "", TableId = 2, Table = null},
            //    new SoloReservation() { IdSoloReservation = 3, Name = "bob", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,18,0,0), Reason = "", TableId = 5, Table = null},
            //   new SoloReservation() { IdSoloReservation = 4, Name = "patrick", Date = new DateTime(2020,10,28), StartTime = new DateTime(2020,10,28,9,30,0), EndTime = new DateTime(2020,10,28,15,0,0), Reason = "", TableId = 3, Table = null}
            //};
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM SoloReservations";
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
                    System.Diagnostics.Debug.WriteLine("id = ", soloRes.IdSoloReservation);
                    soloRes.Name = (string)rdr["Name"];
                    System.Diagnostics.Debug.WriteLine("name = ", soloRes.Name);
                    soloRes.Date = (DateTime)rdr["Date"];
                    soloRes.StartTime = (DateTime)rdr["StartTime"];
                    soloRes.EndTime = (DateTime)rdr["EndTime"];
                    System.Diagnostics.Debug.WriteLine("EndTime = ", soloRes.EndTime);
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model.Add(soloRes);

                    System.Diagnostics.Debug.WriteLine(model.Count());
                }
            }
            conn.Close();
            return View(model);
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
