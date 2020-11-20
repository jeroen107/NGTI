using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class SoloReservationsController : Controller
    {
        // GET: SoloReservationController
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
            string sql = "SELECT StartTime, COUNT(StartTime) AS totaal FROM SoloReservations GROUP BY StartTime HAVING COUNT('totaal') < 10";

            var totals = new List<int>();

            con.Open();
            using (con) ;
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


