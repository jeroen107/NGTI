using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NGTI.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Reservations()
        {
            var reservationList = new List<SoloReservation>()
            {
                new SoloReservation() { IdSoloReservation = 1, Name = "john", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,15,0,0), Reason = "", TableId = 1, Table = null},
                new SoloReservation() { IdSoloReservation = 2, Name = "sponge", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,20,0,0), Reason = "", TableId = 2, Table = null},
                new SoloReservation() { IdSoloReservation = 3, Name = "bob", Date = new DateTime(2020,10,27), StartTime = new DateTime(2020,10,27,9,30,0), EndTime = new DateTime(2020,10,27,18,0,0), Reason = "", TableId = 5, Table = null},
                new SoloReservation() { IdSoloReservation = 4, Name = "patrick", Date = new DateTime(2020,10,28), StartTime = new DateTime(2020,10,28,9,30,0), EndTime = new DateTime(2020,10,28,15,0,0), Reason = "", TableId = 3, Table = null}
            };
            return View(reservationList);
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
