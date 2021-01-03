using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using NGTI.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace NGTI.Controllers
{

    public class AdminController : Controller
    {
        //sql connectionstring
        public string connectionString;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
            this.connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        // index view
        public IActionResult Index()
        {
            return View();
        }
        //all reservations view
        public IActionResult Reservations()
        {
            //get all reservations solo/group
            string sql = "SELECT * FROM SoloReservations ORDER BY Date ASC";
            string sql2 = "SELECT * FROM GroupReservations ORDER BY Date ASC";
            List<SoloReservation> solo = SqlMethods.getSoloReservations(sql);
            List<GroupReservation> group = SqlMethods.getGroupReservations(sql2);

            // send object of reservationlists to view
            var model = new ReservationsViewModel() { soloList = solo, groupList = group };
            return View(model);
        }
        //users view
        public IActionResult ListUsers(string search)
        {
            System.Diagnostics.Debug.WriteLine($"search = {search}");
            var users = userManager.Users.Where(x => x.Email.Contains(search) || search == "" || search == null);

            return View(users);
        }

        //edit chosen user
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View();
            }

            var model = new Employee
            {
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
            var user = await userManager.FindByIdAsync(model.Id);

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

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }
                return View(model);
            }


        }
        //check if reservation is solo or group and redirect
        public IActionResult Details(int id, string type) 
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
        // View details of soloreservation
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
                    soloRes.TimeSlot = (string)rdr["TimeSlot"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model = soloRes;
                }
            }
            conn.Close();
            return View(model);
        }
        //View details of groupreservation
        public IActionResult DetailsGroup(int id)
        {
            GroupReservation model = SqlMethods.getGroupReservation("SELECT * FROM GroupReservations WHERE IdGroupReservation = " + id);
            
            return View(model);
        }

        //check if type of reservation solo/group then redirect to delete view
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
        //delete soloreservation
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
                    soloRes.TimeSlot = (string)rdr["TimeSlot"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model = soloRes;
                }
            }
            conn.Close();
            return View(model);
        }
        //delete groupreservation
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
                    soloRes.TimeSlot = (string)rdr["TimeSlot"];
                    soloRes.Reason = (string)rdr["Reason"];
                    soloRes.TableId = (int)rdr["TableId"];
                    model = soloRes;
                }
            }
            conn.Close();
            return View(model);
        }

        //Delete reservation and go back to reservation view  
        [HttpPost]
        public IActionResult DeleteConfirmed(int id, string type)
        {
            System.Diagnostics.Debug.WriteLine("deleteConfirmed : [" + id + "] [" + type + "]");
            string sql = "";
            if (type == "Solo")
            {
                sql = "DELETE FROM SoloReservations WHERE IdSoloReservation = " + id;               
            }
            else if (type == "Group")
            {
                sql = "DELETE FROM GroupReservations WHERE IdGroupReservation = " + id;
            }
            SqlMethods.QueryVoid(sql);
            return RedirectToAction("Reservations");
        }
        //editsolo view
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
        //view covidmeasure
        public IActionResult covidmeasure()
        {
            int limit = SqlMethods.QueryLimit();
            TempData["limit"] = limit;
            return View();
        }
        //change limit integer 
        [HttpPost]
        public IActionResult covidmeasure(string limit)
        {
            int newLimit = Convert.ToInt32(limit);
            System.Diagnostics.Debug.WriteLine(limit);
            SqlMethods.QueryVoid("UPDATE Limit SET limit = " + limit);
            return RedirectToAction("Index");
        }
        //user downloads txt of reservations
        public FileStreamResult CreateFile()
        {
            DateTime w = DateTime.Now;
            string data = $"{w} reservations of this week\n\n";
            data += "--SoloReservations--\n\n";
            List<SoloReservation> solo = SqlMethods.getSoloReservations("SELECT * FROM SoloReservations WHERE Date >= CAST(GETDATE() AS Date) AND Date <= CAST(GETDATE() + 7 AS Date) ORDER BY Date ASC;");
            List<GroupReservation> group = SqlMethods.getGroupReservations("SELECT * FROM GroupReservations WHERE Date >= GETDATE() AND Date <= GETDATE() + 7 ORDER BY Date ASC;");
            foreach (SoloReservation res in solo)
            {
                data += "{\n";
                data += $"  IdSoloReservation = {res.IdSoloReservation} \n";
                data += $"  Date = {res.Date} \n";
                data += $"  Name = {res.Name} \n";
                data += $"  TimeSlot = {res.TimeSlot} \n";
                data += $"  Reason = {res.Reason} \n";
                data += $"  TableId = {res.TableId} \n";
                data += "}\n";
            }
            data += "\n--GroupReservations--\n\n";
            foreach (GroupReservation res in group)
            {
                data += "{\n";
                data += $"  IdGroupReservation = {res.IdGroupReservation} \n";
                data += $"  Date = {res.Date} \n";
                data += $"  Name = {res.Name} \n";
                data += $"  TeamName = {res.Teamname} \n";
                data += $"  TimeSlot = {res.TimeSlot} \n";
                data += $"  Reason = {res.Reason} \n";
                data += $"  TableId = {res.TableId} \n";
                data += "}\n";
            }
            var bytearray = Encoding.ASCII.GetBytes(data);
            var stream = new System.IO.MemoryStream(bytearray);
            return File(stream, "text/plain", w.ToString()+".txt");
        }
    }
}