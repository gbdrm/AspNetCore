using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Data;
using AspNetCore.Models;

namespace AspNetCore.Controllers
{
    public class TestPackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestPackagesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: TestPackages
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestPackages.ToListAsync());
        }

        // GET: TestPackages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id);
            if (testPackage == null)
            {
                return NotFound();
            }

            return View(testPackage);
        }

        // GET: TestPackages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TestPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestPackageId,Description,TimeCreated,UserId,Viewed")] TestPackage testPackage)
        {
            if (ModelState.IsValid)
            {
                testPackage.TestPackageId = Guid.NewGuid();
                _context.Add(testPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(testPackage);
        }

        // GET: TestPackages/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id);
            if (testPackage == null)
            {
                return NotFound();
            }
            return View(testPackage);
        }

        // POST: TestPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TestPackageId,Description,TimeCreated,UserId,Viewed")] TestPackage testPackage)
        {
            if (id != testPackage.TestPackageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestPackageExists(testPackage.TestPackageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(testPackage);
        }

        // GET: TestPackages/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id);
            if (testPackage == null)
            {
                return NotFound();
            }

            return View(testPackage);
        }

        // POST: TestPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id);
            _context.TestPackages.Remove(testPackage);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TestPackageExists(Guid id)
        {
            return _context.TestPackages.Any(e => e.TestPackageId == id);
        }
    }
}
