﻿using Microsoft.AspNetCore.Mvc;

namespace TicketingSystem.UI.Areas.Person.Controllers
{
    [Area("Person")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProfileSettings() { 
            return View();
        }

        public IActionResult Profile() {
            return View();
        }
    }
}
