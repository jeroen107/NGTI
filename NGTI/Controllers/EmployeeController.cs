using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class EmployeeController : Controller
    {
        EmployeeDBAccessLayer empdb = new EmployeeDBAccessLayer();

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind] Employee employeeEntities)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string resp = empdb.AddEmployeeRecord(employeeEntities);
                    TempData["msg"] = resp;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return View();
        }
    }
}

