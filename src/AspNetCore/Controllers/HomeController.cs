using System;
using System.Linq;
using System.Security.Claims;
using AspNetCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            var model = _db.TestPackages.OrderBy(p=>p.TimeCreated)
                .Include(p => p.User)
                .Include(p=>p.TestItems);
            
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
