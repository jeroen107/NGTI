using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class SoloReservationController : Controller
    {
        // GET: SoloReservationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SoloReservationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SoloReservationController/Edit/5
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
    }
}
