using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;
using NGTI.Models;

namespace NGTI.Controllers
{
    [Authorize]
    public class GroupReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManger;
        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";

        public GroupReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManger = userManager;
        }

        // GET: GroupReservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.GroupReservations.Include(g => g.Table);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: GroupReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupReservation = await _context.GroupReservations
                .Include(g => g.Table)
                .FirstOrDefaultAsync(m => m.IdGroupReservation == id);
            if (groupReservation == null)
            {
                return NotFound();
            }

            return View(groupReservation);
        }

        // GET: GroupReservations/Create
        public IActionResult Create()
        {
            var teams = new List<Team>();
            var fake = new Team();
            fake.TeamName = "Pick A Team";
            fake.Members = 0;
            teams.Add(fake);
            SqlConnection conn = new SqlConnection(connectionString);
            var id = _userManger.GetUserId(HttpContext.User);
            string sql = $"SELECT t.TeamName, COUNT(tm.UserId) AS count FROM Teams t LEFT JOIN TeamMembers tm ON t.TeamName = tm.TeamName WHERE t.teamname IN(SELECT DISTINCT teamname from teammembers WHERE userid = N'{id}') GROUP BY t.TeamName;";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Team>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Team();
                    obj.TeamName = (string)rdr["TeamName"];
                    obj.Members = 1;
                    teams.Add(obj);
                    System.Diagnostics.Debug.WriteLine(teams.Count);
                }
            }
            conn.Close();
            ViewData["TeamName"] = new SelectList(teams, "TeamName", "TeamName");
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId");
            System.Diagnostics.Debug.WriteLine(teams.Count);
            return View();
        }

        // POST: GroupReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGroupReservation,Name,Teamname,Date,TimeSlot,Reason,TableId")] GroupReservation groupReservation, bool entireWeek, IEnumerable<int> days, int selectedWeek)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            firstDay = correctToMonday(firstDay);
            firstDay = firstDay.AddDays(7 * (selectedWeek - 1));
            groupReservation.Date = firstDay;
            System.Diagnostics.Debug.WriteLine("entireweek = " + entireWeek.ToString());
            //modelstate is not valid when trying to set multiple dates
            if (ModelState.IsValid || !ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"testbox = {entireWeek}");
                //reserve whole week automatic
                if (entireWeek == true)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        if (groupReservation.Date >= DateTime.Today)
                        {
                            _context.Add(groupReservation);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine($"added {x}");
                        }
                        groupReservation.Date = groupReservation.Date.AddDays(1);
                        groupReservation.IdGroupReservation = 0;
                    }
                    return RedirectToAction(nameof(Index));
                }
                //reserve chosen days
                else
                {
                    foreach (int day in days)
                    {
                        groupReservation.Date = groupReservation.Date.AddDays(day);
                        if (groupReservation.Date >= DateTime.Today)
                        {
                            _context.Add(groupReservation);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine($"added {day}");
                        }
                        groupReservation.Date = firstDay;
                        groupReservation.IdGroupReservation = 0;
                    }
                    return RedirectToAction(nameof(Index));

                }
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", groupReservation.TableId);
            return View(groupReservation);
        }
        DateTime correctToMonday(DateTime fday)
        {
            DayOfWeek dow = fday.DayOfWeek;
            if (dow == DayOfWeek.Monday)
            {
                return fday;
            }
            else
            {
                return correctToMonday(fday.AddDays(-1));
            }
        }

        // GET: GroupReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("IdGroupReservation,Name,Teamname,Date,TimeSlot,Reason,TableId")] GroupReservation groupReservation)
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
                    if (!GroupReservationExists(groupReservation.IdGroupReservation))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", groupReservation.TableId);
            return View(groupReservation);
        }

        // GET: GroupReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupReservation = await _context.GroupReservations
                .Include(g => g.Table)
                .FirstOrDefaultAsync(m => m.IdGroupReservation == id);
            if (groupReservation == null)
            {
                return NotFound();
            }

            return View(groupReservation);
        }

        // POST: GroupReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupReservation = await _context.GroupReservations.FindAsync(id);
            _context.GroupReservations.Remove(groupReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupReservationExists(int id)
        {
            return _context.GroupReservations.Any(e => e.IdGroupReservation == id);
        }
    }
}