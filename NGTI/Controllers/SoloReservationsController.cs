﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;
using NGTI.Models;

namespace NGTI.Controllers
{
    [Authorize]
    public class SoloReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

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
            }
            ViewData["TableId"] = new SelectList(_context.Tables, "TableId", "TableId", soloReservation.TableId);
            return View(soloReservation);
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
    }
}