using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Storage;

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
            string sql2 = "SELECT * FROM GroupReservations ORDER BY Date ASC";
            string[] sqls = new string[2] { sql, sql2 };

            var solo = new List<SoloReservation>();
            var group = new List<GroupReservation>();

            conn.Open();
            using (conn)
            {
                //read all reservations and add them to tuple<solo,group>
                for (int x = 0; x < 2; x++) {
                    SqlCommand cmd = new SqlCommand(sqls[x], conn);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (x == 0)
                    {
                        while (rdr.Read())
                        {
                            var res = new SoloReservation();
                            res.IdSoloReservation = (int)rdr["IdSoloReservation"];
                            res.Name = (string)rdr["Name"];
                            res.Date = (DateTime)rdr["Date"];
                            res.StartTime = (DateTime)rdr["StartTime"];
                            res.EndTime = (DateTime)rdr["EndTime"];
                            res.Reason = (string)rdr["Reason"];
                            res.TableId = (int)rdr["TableId"];
                            solo.Add(res);
                        }
                    }
                    else if (x == 1)
                    {
                        while (rdr.Read())
                        {
                            var res = new GroupReservation();
                            res.IdGroupReservation = (int)rdr["IdGroupReservation"];
                            res.Teamname = (string)rdr["Teamname"];
                            res.Name = (string)rdr["Name"];
                            res.Date = (DateTime)rdr["Date"];
                            res.StartTime = (DateTime)rdr["StartTime"];
                            res.EndTime = (DateTime)rdr["EndTime"];
                            res.Reason = (string)rdr["Reason"];
                            res.TableId = (int)rdr["TableId"];
                            group.Add(res);
                        }
                    }
                }
            }
            conn.Close();
            var model = new ReservationsViewModel() { soloList = solo, groupList = group };
            return View(model);
        }
        public IActionResult Details(int id, string type) //check if obj is solo or group and redirect
        {
            if (type == "Solo")
            {
                return RedirectToAction("DetailsSolo", new { id = id });
            }
            else if (type == "Group")
            {
                return RedirectToAction("DetailsGroup", new { id = id });
            }
            else
            {
                return NotFound();
            }
        }
        // Details of solo or group model
        public IActionResult DetailsSolo(int id)
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
        public IActionResult DetailsGroup(int id)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM GroupReservations WHERE IdGroupReservation = " + id;
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
                    res.StartTime = (DateTime)rdr["StartTime"];
                    res.EndTime = (DateTime)rdr["EndTime"];
                    res.Reason = (string)rdr["Reason"];
                    res.TableId = (int)rdr["TableId"];
                    model = res;
                }
            }
            conn.Close();
            return View(model);
        }

        // GET: /<controller>/
        public IActionResult Delete(int id, string type)
        {
            if (type == "Solo")
            {
                return RedirectToAction("DeleteSolo", new { id = id, type = type });
            }
            else if (type == "Group")
            {
                return RedirectToAction("DeleteGroup", new { id = id, type = type });
            }
            else
            {
                return NotFound();
            }
        }
        public IActionResult DeleteSolo(int id, string type)
        {
            System.Diagnostics.Debug.WriteLine("deleteSolo : [" + id + "] [" + type + "]");
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
        public IActionResult DeleteGroup(int id, string type)
        {
            System.Diagnostics.Debug.WriteLine("deleteGroup : [" + id + "] [" + type + "]");

            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM GroupReservations WHERE IdGroupReservation = " + id;
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new GroupReservation();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var soloRes = new GroupReservation();
                    soloRes.IdGroupReservation = (int)rdr["IdGroupReservation"];
                    soloRes.Name = (string)rdr["Name"];
                    soloRes.Teamname = (string)rdr["Teamname"];
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
        [HttpPost]
        public IActionResult DeleteConfirmed(int id, string type)
        {
            System.Diagnostics.Debug.WriteLine("deleteConfirmed : [" + id + "] [" + type + "]");

            if (type == "Solo")
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
            }
            else if (type == "Group")
            {
                SqlConnection conn = new SqlConnection(connectionString);
                string sql = "DELETE FROM GroupReservations WHERE IdGroupReservation = " + id;
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                using (conn)
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                }
                conn.Close();
            }
            return RedirectToAction("Reservations");
        }
        public string Edit(int id)
        {
            return "Hello, "+id;
        }
    }
}
