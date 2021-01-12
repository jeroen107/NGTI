using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;
//using MongoDB.Driver.Core.Configuration;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class NotificationController : Controller
    {
        /*private readonly UserManager<ApplicationUser> _userManger;*/

        string connectionString = "Server=(localdb)\\mssqllocaldb;Database=NGTI;Trusted_Connection=True;MultipleActiveResultSets=true";

        /*public NotificationController(UserManager<ApplicationUser> userManager)
        {
            _userManger = userManager;
        }*/

        // GET: NotificationController
        public ActionResult Index()
        {
            SqlConnection conn = new SqlConnection(connectionString);
            //var id = _userManger.GetUserId(HttpContext.User);
            string sql = "SELECT * FROM Notifications ORDER BY Date ASC";
            SqlCommand cmd = new SqlCommand(sql, conn);
            var model = new List<Notification>();
            conn.Open();
            using (conn)
            {
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var obj = new Notification();
                    obj.title = (string)rdr["title"];
                    obj.body = (string)rdr["body"];
                    model.Add(obj);
                }
            }
            conn.Close();
            return View(model);
        }

        // GET: NotificationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NotificationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotificationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: NotificationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NotificationController/Edit/5
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

        // GET: NotificationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NotificationController/Delete/5
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
    }
}
