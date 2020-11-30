using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using NGTI.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGTI.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reservations()
        {
            // Sql connection
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT * FROM SoloReservations ORDER BY starttime ASC";
            string sql2 = "SELECT * FROM GroupReservations ORDER BY starttime ASC";
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
                new SoloReservation() { IdSoloReservation = 1, Name = "john", StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,15,0,0), Reason = "", TableId = 1, Table = null},
                new SoloReservation() { IdSoloReservation = 2, Name = "sponge", StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,20,0,0), Reason = "", TableId = 2, Table = null},
                new SoloReservation() { IdSoloReservation = 3, Name = "bob", StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,18,0,0), Reason = "", TableId = 5, Table = null},
                new SoloReservation() { IdSoloReservation = 4, Name = "patrick", StartTime = new DateTime(2020,10,28,9,30,0), EndTime = new DateTime(2020,10,28,15,0,0), Reason = "", TableId = 3, Table = null}
            };
            return View(reservationList);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View();
            }

            var model = new Employee
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
                    soloRes.StartTime = (DateTime)rdr["StartTime"];
                    soloRes.EndTime = (DateTime)rdr["EndTime"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model = soloRes;
                }
            }
            conn.Close();
                Id = user.Id,
                Email = user.Email,
                BHV = user.BHV,
                Admin = user.Admin,

            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(Employee model)
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

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View();
            }
            else
            {
                user.Email = model.Email;
                user.BHV = model.BHV;
                user.Admin = model.Admin;

                var result = await userManager.UpdateAsync(user);
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

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }
                return View(model);
            }
            
            
                    var soloRes = new GroupReservation();
                    soloRes.IdGroupReservation = (int)rdr["IdGroupReservation"];
                    soloRes.Name = (string)rdr["Name"];
                    soloRes.Teamname = (string)rdr["Teamname"];
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
        public ActionResult EditSolo(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSolo(int id, IFormCollection collection)
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

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGroup(int id, [Bind("IdGroupReservation,Name,Teamname,StartTime,EndTime,Reason,TableId")] GroupReservation groupReservation)
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

        private bool GroupReservationExists(int id)
        {
            return _context.GroupReservations.Any(e => e.IdGroupReservation == id);
        }

        public IActionResult covidmeasure()
        {
            int limit = SqlMethods.QueryLimit();
            TempData["limit"] = limit;
            return View();
        }
        [HttpPost]
        public IActionResult covidmeasure(string limit)
        {
            int newLimit = Convert.ToInt32(limit);
            System.Diagnostics.Debug.WriteLine(limit);
            SqlMethods.QueryVoid("UPDATE Limit SET limit = " + limit);
            return RedirectToAction("Index");
        }
    }
}