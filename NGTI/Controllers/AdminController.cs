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
        //sql connection var
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reservations()
        {
            // Sql connection
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM SoloReservations ORDER BY Date ASC";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<SoloReservation>();
            conn.Open();
            using (conn)
            {
                //read all SoloReservations and add them to List<SoloReservation>
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var soloRes = new SoloReservation();
                    soloRes.IdSoloReservation = (int)rdr["IdSoloReservation"];
                    soloRes.Name = (string)rdr["Name"];
                    soloRes.Date = (DateTime)rdr["Date"];
                    soloRes.StartTime = (DateTime)rdr["StartTime"];
                    soloRes.EndTime = (DateTime)rdr["EndTime"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model.Add(soloRes);
                }
            }
            conn.Close();
            return View(model);
        }
        public IActionResult Details(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM SoloReservations WHERE IdSoloReservation = " + id;
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new SoloReservation();
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
                    soloRes.StartTime = (DateTime)rdr["StartTime"];
                    soloRes.EndTime = (DateTime)rdr["EndTime"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model = soloRes;
                }
            }
            conn.Close();
            return View(model);
        }

        
        // GET: /<controller>/
        public IActionResult Delete(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM SoloReservations WHERE IdSoloReservation = "+ id;
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new SoloReservation();
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
                    soloRes.StartTime = (DateTime)rdr["StartTime"];
                    soloRes.EndTime = (DateTime)rdr["EndTime"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model = soloRes;
                }
            }
            conn.Close();
            return View(model);
        }
        // POST: /<controller>/
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "DELETE FROM SoloReservations WHERE IdSoloReservation = " + id;
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
            }
            conn.Close();
            return RedirectToAction("Reservations");
        }
    }
}
