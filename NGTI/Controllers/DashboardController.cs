using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;
//using MongoDB.Driver.Core.Configuration;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
        
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Overview()
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

        public ActionResult Index()
        {
            return View();
        }

        // GET: SoloReservationController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soloReservation = await _context.SoloReservations
                .Include(s => s.Table)
                .FirstOrDefaultAsync(m => m.IdSoloReservation == id);
            if (soloReservation == null)
            {
                return NotFound();
            }

            return View(soloReservation);
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
        public async Task<IActionResult> EditSolo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soloReservation = await _context.SoloReservations.FindAsync(id);
            if (soloReservation == null)
            {
                return NotFound();
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", soloReservation.TableId);
            return View(soloReservation);
        }

        // POST: SoloReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSolo(int id, [Bind("IdSoloReservation,Name,Date,TimeSlot,Reason,TableId")] SoloReservation soloReservation)
        {
            if (id != soloReservation.IdSoloReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soloReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", soloReservation.TableId);
            return View(soloReservation);
        }

        // GET: GroupReservations/Edit/5
        public async Task<IActionResult> EditGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupReservation = await _context.GroupReservations.FindAsync(id);
            if (groupReservation == null)
            {
                return NotFound();
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", groupReservation.TableId);
            return View(groupReservation);
        }

        // POST: GroupReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, [Bind("IdGroupReservation,Name,Teamname,Date,TimeSlot,Reason,TableId")] GroupReservation groupReservation)
        {
            if (id != groupReservation.IdGroupReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", groupReservation.TableId);
            return View(groupReservation);
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
