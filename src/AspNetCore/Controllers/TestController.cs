using System;
using System.Linq;
using AspNetCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Controllers
{
    public class TestController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(Guid? gameId = null, Guid? taskId = null)
        {
            var package = _context.TestPackages.SingleOrDefault(p=>p.TestPackageId == gameId);

            return View(package);
        }

        public IActionResult Start()
        {
            var firstTest = _context.TestPackages.Include(p=>p.TestItems).FirstOrDefault();
            if (firstTest?.TestItems != null)
            {
                var firstTask = firstTest.TestItems.FirstOrDefault();
                return View("Index", firstTask);
            }

            return View("Index");
        }
    }
}