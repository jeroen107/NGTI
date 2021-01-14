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
using System.Data.SqlClient;

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
            var applicationDbContext = _context.SoloReservations; //.Include(s => s.Seat)
            var temp = applicationDbContext.OrderBy(x => x.Date).ToList();
            return View( temp.ToList());
        }

        // GET: SoloReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var soloReservation = await _context.SoloReservations
                //.Include(s => s.Seat)
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
            //ViewData["Seat"] = new SelectList(_context.Seats, "Seat", "Seat");
            return View();
        }

        public bool limitTest(SoloReservation res)
        {
            int limit = SqlMethods.QueryLimit();
            TempData["limit"] = limit;

            var applicationDbContext = _context.SoloReservations;
            int i = 0;
            foreach(SoloReservation solo in applicationDbContext)
            {
                if (solo.Date == res.Date && solo.TimeSlot == res.TimeSlot)
                {
                    i++;
                    System.Diagnostics.Debug.WriteLine(i);
                }
            }
            System.Diagnostics.Debug.WriteLine(i);

            if( i < limit)
            {
                return false;
            }

            return true;
        }
        // POST: SoloReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSoloReservation,Name,TimeSlot,Reason,Seat")] SoloReservation soloReservation, bool entireWeek, IEnumerable<int> days, int selectedWeek)
        {
            if (soloReservation.Reason == null)
            {
                soloReservation.Reason = "no reason";
            }
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            firstDay = correctToMonday(firstDay);
            firstDay = firstDay.AddDays(7 * (selectedWeek - 1));
            soloReservation.Date = firstDay;
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
                        if (soloReservation.Date >= DateTime.Today && !limitTest(soloReservation))
                        {
                            _context.Add(soloReservation);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine($"added {x}");

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
                                Location = $"tafelnummer: {soloReservation.Seat}",
                                Description = soloReservation.Reason,

                                Start = new EventDateTime()
                                {

                                    DateTime = soloReservation.Date,
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

                        }
                        soloReservation.Date = soloReservation.Date.AddDays(1);
                        soloReservation.IdSoloReservation = 0;
                    }
                    return RedirectToAction(nameof(Index));
                }
                //reserve chosen days
                else
                {
                    foreach (int day in days)
                    {
                        soloReservation.Date = soloReservation.Date.AddDays(day);
                        if (soloReservation.Date >= DateTime.Today)
                        {
                            _context.Add(soloReservation);
                            await _context.SaveChangesAsync();
                            System.Diagnostics.Debug.WriteLine($"added {day}");


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
                                Location = $"tafelnummer: {soloReservation.Seat}",
                                Description = soloReservation.Reason,

                                Start = new EventDateTime()
                                {

                                    DateTime = soloReservation.Date,
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

                        }
                        soloReservation.Date = firstDay;
                        soloReservation.IdSoloReservation = 0;
                    }
                    return RedirectToAction(nameof(Index));

                }

            }
            //ViewData["Seat"] = new SelectList(_context.Seats, "Seat", "Seat", soloReservation.Seat);
            return View(soloReservation);
        }
       /* public async Task<IActionResult> Create([Bind("IdSoloReservation,Name,Date,TimeSlot,Reason,TableId")] SoloReservation soloReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soloReservation);
                await _context.SaveChangesAsync();
                //SoloReservation obj = new SoloReservation();

                

                return RedirectToAction(nameof(Index));
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", soloReservation.TableId);
            return View(soloReservation);
        }*/
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
            //ViewData["Seat"] = new SelectList(_context.Seats, "Seat", "Seat", soloReservation.Seat);
            return View(soloReservation);
        }

        // POST: SoloReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("IdSoloReservation,Name,Date,TimeSlot,Reason,Seat")] SoloReservation soloReservation)
        {
            Console.WriteLine("arrived at edit");
            System.Diagnostics.Debug.WriteLine(soloReservation.IdSoloReservation);
            
            

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
            //ViewData["Seat"] = new SelectList(_context.Seats, "Seat", "Seat", soloReservation.Seat);
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
                //.Include(s => s.Seat)
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
        public IActionResult DeleteConfirmed(int id, string type)
        {
            System.Diagnostics.Debug.WriteLine("deleteConfirmed : [" + id + "] [" + type + "]");

            if (type == "solo")
            {
                string sql = "DELETE FROM SoloReservations WHERE IdSoloReservation = " + id;
                SqlMethods.QueryVoid(sql);
            }
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