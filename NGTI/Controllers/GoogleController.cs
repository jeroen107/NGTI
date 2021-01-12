using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Web;
using NGTI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
namespace NGTI.Controllers
{
    public class GoogleController : Controller
    {
        public SoloReservation Reservation;
        public List<string> GoogleEvents = new List<string>();
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET NGTI";
        
        // GET: GoogleController
        public ActionResult GoogleEvent()
        {
            
            UserCredential credential;

            Reservation = (SoloReservation)TempData["SoloReservation"];



            string path = Path.Combine(Directory.GetCurrentDirectory(), "credentials.json");
            using (var stream =
                new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

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
                Location = $"{Reservation.Seat}",
                Description = Reservation.Reason,

                Start = new EventDateTime()
                {

                    DateTime = DateTime.Parse($"{Reservation.Date}T08:00:00-12:00"),
                    TimeZone = "America/Los_Angeles",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse($"{Reservation.Date}T08:00:00-12:00"),
                    TimeZone = "America/Los_Angeles",
                },


            };
            if (Reservation.TimeSlot == "Morning")
            {
                newEvent.Start.DateTime = DateTime.Parse($"{Reservation.Date}T08:00:00-12:00");

            }
            else if (Reservation.TimeSlot == "Afternoon")
            {
                newEvent.Start.DateTime = DateTime.Parse($"{Reservation.Date}T12:00:00-16:00");
            }
            else if (Reservation.TimeSlot == "Evening")
            {
                newEvent.Start.DateTime = DateTime.Parse($"{Reservation.Date}T16:00:00-20:00");
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


            return RedirectToAction("Index", "SoloReservationController");
        }


        public void CalenderEvents()
        {
            


        }
    }
}
