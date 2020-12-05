using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NGTI.Data;
using NGTI.Models;

namespace NGTI.Controllers
{
    public class Reservations : Controller
    {
        public IActionResult Planner()
        {
            return View();
        }
    }
   }



