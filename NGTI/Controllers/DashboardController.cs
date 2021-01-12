using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using MongoDB.Driver.Core.Configuration;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";

        public DashboardController( UserManager<ApplicationUser> userManager)
        {
            _userManger = userManager;
        }
        public IActionResult Overview()
        {
            var id = _userManger.GetUserId(HttpContext.User);
            // Sql connection
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM SoloReservations Where Date <= CURRENT_TIMESTAMP + 7 and Date >= CURRENT_TIMESTAMP ORDER BY Date ASC";
            string sql2 = "SELECT * FROM Groupreservations Where Date <= CURRENT_TIMESTAMP + 7 and Date >= CURRENT_TIMESTAMP ORDER BY Date ASC";
            string sql3 = $"SELECT t.TeamName, COUNT(tm.UserId) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName WHERE t.teamname IN(SELECT DISTINCT teamname from teammembers WHERE userid = N'{id}') GROUP BY t.TeamName;";
            string[] sqls = new string[3] { sql, sql2, sql3 };

            var solo = new List<SoloReservation>();
            var group = new List<GroupReservation>();
            var mygroups = new List<string>();
            var myGroupRes = new List<GroupReservation>();

            conn.Open();
            using (conn)
            {
                //read all reservations and add them to tuple<solo,group>
                for (int x = 0; x < 3; x++)
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
                            res.Seat = (string)rdr["Seat"];
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
                            res.Seat = (string)rdr["Seat"];
                            group.Add(res);
                        }
                    }
                    else if (x == 2)
                    {
                        while (rdr.Read())
                        {
                            string TeamName = (string)rdr["TeamName"];
                            mygroups.Add(TeamName);
                        }
                    }
                }
            }
            conn.Close();
            foreach(GroupReservation res in group)
            {
                if (mygroups.Contains(res.Teamname))
                {
                    myGroupRes.Add(res);
                }
            }
            var model = new DashboardViewModel() { soloList = solo, groupList = group, mygroupList = myGroupRes };
            return View(model);
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: SoloReservationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SoloReservationController/Create
        SoloReservationDBAccesLayer SolResdb = new SoloReservationDBAccesLayer();

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind] SoloReservation SoloReservationEntities)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string resp = SolResdb.AddSoloReservationRecord(SoloReservationEntities);
                    TempData["msg"] = resp;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }

        // GET: SoloReservationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SoloReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SoloReservationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SoloReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ReservationCheck()
        {
            SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true");
            string sql = "SELECT Date, COUNT(Date) AS totaal FROM SoloReservations GROUP BY Date HAVING COUNT('totaal') < 10";

            var totals = new List<int>();

            con.Open();
            using (con)
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int total = (int)rdr["totaal"];
                    totals.Add(total);
                }
            }
            con.Close();

            foreach (int total in totals)
            {
                if (total > 5)
                {
                    return NotFound();
                }
            }
            return Index();
        }
    }
}
