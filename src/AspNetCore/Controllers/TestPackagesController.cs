using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Data;
using AspNetCore.Models;
using AspNetCore.Models.TestsViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.Controllers
{
    [Authorize]
    public class TestPackagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Guid ThisUser => Helper.GetCurrentUserId(User) ?? Guid.Empty;

        public TestPackagesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        public IActionResult Finished()
        {
            Guid? userId = Helper.GetCurrentUserId(User);
            if (userId.HasValue)
            {
                var model = _context.TestResults.Where(r => r.UserId == userId);
                return View(model.ToList());
            }

            return View();
        }

        public IActionResult Index()
        {
            Guid? userId = Helper.GetCurrentUserId(User);
            if (userId.HasValue)
            {
                var packages = _context.TestPackages.Where(r => r.UserId == userId.Value).ToList();
                var model = GetPackagesModel(packages);

                return View(model);
            }

            return View();
        }

        private IEnumerable<TestPackagesViewModel> GetPackagesModel(IEnumerable<TestPackage> packages)
        {
            foreach (var testPackage in packages)
            {
                var tests = _context.TestItems.Where(i => i.TestPackageId == testPackage.TestPackageId);
                TestPackagesViewModel modelItem = new TestPackagesViewModel
                {
                    TestItems = tests,
                    TestPackage = testPackage
                };

                yield return modelItem;
            }
        }

        // GET: TestPackages/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id && m.UserId == ThisUser);
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
        public async Task<IActionResult> Create([Bind("Name,Description")] TestPackage testPackage)
        {
            if (ModelState.IsValid)
            {
                testPackage.TestPackageId = Guid.NewGuid();
                testPackage.TimeCreated = DateTime.Now;
                Guid? userId = Helper.GetCurrentUserId(User);
                if (userId.HasValue)
                {
                    testPackage.UserId = userId.Value;
                }
                
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

            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id && m.UserId == ThisUser);
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
        public async Task<IActionResult> Edit(Guid id, [Bind("TestPackageId,Name,Description")] TestPackage testPackage)
        {
            if (id != testPackage.TestPackageId && testPackage.UserId == ThisUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.TestPackages.Attach(testPackage);
                    _context.Entry(testPackage).Property(p => p.Name).IsModified = true;
                    _context.Entry(testPackage).Property(p => p.Description).IsModified = true;
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

            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id && m.UserId == ThisUser);
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
            var testPackage = await _context.TestPackages.SingleOrDefaultAsync(m => m.TestPackageId == id && m.UserId == ThisUser);
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
