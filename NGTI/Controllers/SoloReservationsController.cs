using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;
using NGTI.Models;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace NGTI.Controllers
{
    [Authorize]
    public class SoloReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public List<string> GoogleEvents = new List<string>();
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET NGTI";
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<HomeController> _logger;
        public SoloReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SoloReservations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SoloReservations.Include(s => s.Table);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SoloReservations/Details/5
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

        // GET: SoloReservations/Create
        public IActionResult Create()
        {
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId");
            return View();
        }

        // POST: SoloReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSoloReservation,Name,TimeSlot,Reason,TableId")] SoloReservation soloReservation,bool entireWeek, IEnumerable<int> days, int selectedWeek)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            firstDay = correctToMonday(firstDay);
            firstDay = firstDay.AddDays(7 * (selectedWeek - 1));
            soloReservation.Date = firstDay;
            System.Diagnostics.Debug.WriteLine("entireweek = "+entireWeek.ToString()); 
            //modelstate is not valid when trying to set multiple dates
            if (ModelState.IsValid || !ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine($"testbox = {entireWeek}");
                //reserve whole week automatic
                if (entireWeek == true)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        if (soloReservation.Date >= DateTime.Today)
                        {
                            _context.Add(soloReservation);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine($"added {x}");
                        }
                        soloReservation.Date = soloReservation.Date.AddDays(1);
                        soloReservation.IdSoloReservation = 0;
                    }
                    return RedirectToAction(nameof(Index));
                }
                //reserve chosen days
                else
                {
                    foreach(int day in days)
                    {
                        soloReservation.Date = soloReservation.Date.AddDays(day);
                        if (soloReservation.Date >= DateTime.Today)
                        {
                            _context.Add(soloReservation);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine($"added {day}");
                        }
                        soloReservation.Date = firstDay;
                        soloReservation.IdSoloReservation = 0;
                    }
                    return RedirectToAction(nameof(Index));

                }
        public async Task<IActionResult> Create([Bind("IdSoloReservation,Name,Date,TimeSlot,Reason,TableId")] SoloReservation soloReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soloReservation);
                await _context.SaveChangesAsync();
                //SoloReservation obj = new SoloReservation();

                UserCredential credential;

                //var userId = User.Identity.GetUserId();
                //var user = UserManager<>.FindByIdAsync(userId);


                //string path = Path.Combine(Directory.GetCurrentDirectory(), "credentials.json");
                using (var stream =
                    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.

                    //var userId = this.User.Identity.GetUserId();
                    //var dbContext = new ApplicationDbContext(DbContextOptions<ApplicationDbContext> options);
                    //var user = dbContext.Set<ApplicationUser>().Find(userId);
                   

                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    //user.Token = "New value";
                }
                /* 
                 *   public List<Employee> GetMembers(string name)
                 {
                 SqlConnection conn = new SqlConnection(connectionString);
                 string sql = "SELECT a.Id, a.Email, a.BHV, a.Admin FROM AspNetUsers a JOIN TeamMembers tm ON a.Id = tm.UserId WHERE tm.TeamName = '" + name + "'";
                 SqlCommand cmd = new SqlCommand(sql, conn);
                 var model = new List<Employee>();
                 conn.Open();
                 using (conn)
                 {
                     SqlDataReader rdr = cmd.ExecuteReader();
                     while (rdr.Read())
                     {
                         var obj = new Employee();
                         obj.Id = (string)rdr["Id"];
                         obj.Email = (string)rdr["Email"];
                         obj.BHV = (bool)rdr["BHV"];
                         obj.Admin = (bool)rdr["Admin"];
                         model.Add(obj);
                     }
                 }
                 conn.Close();
                 return model;
             }
                     * 
                 * 
                 * 
                 * var userStore = new UserStore<ApplicationUser>(new
                                ApplicationDbContext());
                 var appManager = new UserManager<ApplicationUser>(userStore);

                 // here you can do a foreach loop and get the email and assign new datas
                 foreach (var i in model)
                 {
                     var currentUser = appManager.FindByEmail(i.Email);

                     // here you can assign the updated values
                     currentUser.userFname = i.userFname;
                     // and rest fields are goes here
                     await appManager.UpdateAsync(currentUser);
                 }
                 var ctx = userStore.Context;
                 ctx.SaveChanges();
                 // now you can redirect to some other method or-else you can return 
                 // to this view itself by returning the data

                 return RedirectToAction("SomeActionMethod");*/

                // Create Google Calendar API service.
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // Define parameters of request.
                Event newEvent = new Event()
                {
                    Summary = "Kantoor reservering",
                    Location = $"tafelnummer: {soloReservation.TableId}",
                    Description = soloReservation.Reason,

                    Start = new EventDateTime()
                    {

                        DateTime= soloReservation.Date,
                        TimeZone = "America/Los_Angeles",
                    },
                    End = new EventDateTime()
                    {
                        DateTime = soloReservation.Date,
                        TimeZone = "America/Los_Angeles",
                    },


                };                                          
                if (soloReservation.TimeSlot == "Morning") 
                {
                    TimeSpan tspanStart = new System.TimeSpan(0, 8, 0, 0);
                    TimeSpan tspanEnd = new System.TimeSpan(0, 12, 0, 0);
                    newEvent.Start.DateTime = newEvent.Start.DateTime + tspanStart;
                    newEvent.End.DateTime = newEvent.End.DateTime + tspanEnd;
                }
                else if (soloReservation.TimeSlot == "Afternoon")
                {
                    TimeSpan tspanStart = new System.TimeSpan(0, 12, 0, 0);
                    TimeSpan tspanEnd = new System.TimeSpan(0, 16, 0, 0);
                    newEvent.Start.DateTime = newEvent.Start.DateTime + tspanStart;
                    newEvent.End.DateTime = newEvent.End.DateTime + tspanEnd;
                }
                else if (soloReservation.TimeSlot == "Evening")
                {
                    TimeSpan tspanStart = new System.TimeSpan(0, 16, 0, 0);
                    TimeSpan tspanEnd = new System.TimeSpan(0, 20, 0, 0);
                    newEvent.Start.DateTime = newEvent.Start.DateTime + tspanStart;
                    newEvent.End.DateTime = newEvent.End.DateTime + tspanEnd;
                }


                String calendarId = "primary";
                EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                Event createdEvent = request.Execute();
                Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);




                // List events.
                /* Events events = request.Execute();
                 Console.WriteLine("Upcoming events:");
                 if (events.Items != null && events.Items.Count > 0)
                 {
                     foreach (var eventItem in events.Items)
                     {
                         GoogleEvents.Add(eventItem.Summary);
                     }
                 }*/

                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", soloReservation.TableId);
            return View(soloReservation);
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

        // GET: SoloReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("IdSoloReservation,Name,Date,TimeSlot,Reason,TableId")] SoloReservation soloReservation)
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
                    if (!SoloReservationExists(soloReservation.IdSoloReservation))
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
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", soloReservation.TableId);
            return View(soloReservation);
        }

        // GET: SoloReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: SoloReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var soloReservation = await _context.SoloReservations.FindAsync(id);
            _context.SoloReservations.Remove(soloReservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoloReservationExists(int id)
        {
            return _context.SoloReservations.Any(e => e.IdSoloReservation == id);
        }

        protected void MyEvent(object sender, EventArgs e)
        {
            SendtoCalendar();
        }

        private void SendtoCalendar()
        {
            
        }

    }
}