using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NGTI.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;

namespace NGTI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
                for (int x = 0; x < 2; x++)
                {
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
                            res.TimeSlot = (string)rdr["TimeSlot"];
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
                            res.TimeSlot = (string)rdr["TimeSlot"];
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
