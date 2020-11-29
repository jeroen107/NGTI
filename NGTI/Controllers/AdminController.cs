using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

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
            var reservationList = new List<SoloReservation>()
            {
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

        public IActionResult Delete()
        {
            return View();
        }
    }
}
