using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCore.Controllers
{
    [Authorize]
    public class TestItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public TestItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TestItems
        public async Task<IActionResult> Index(Guid? id)
        {
            IQueryable<TestItem> applicationDbContext = _context.TestItems;
            if (id != null)
            {
                applicationDbContext = applicationDbContext.Where(p => p.TestPackageId == id);
            }

            var model = await applicationDbContext
                .OrderBy(p=>p.Order)
                .Include(t => t.TestPackage)
                .ToListAsync();

            return View(model);
        }

        // GET: TestItems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testItem = await _context.TestItems.SingleOrDefaultAsync(m => m.TestItemId == id);
            if (testItem == null)
            {
                return NotFound();
            }

            return View(testItem);
        }

        // GET: TestItems/Create
        public IActionResult Create()
        {
            ViewData["TestPackageId"] = new SelectList(GetTestPackages(), "TestPackageId", "Name");
            ViewData["TestTypes"] = new SelectList(Enum.GetValues(typeof(TestType)).Cast<TestType>());
            return View();
        }

        // POST: TestItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestItemId,Answer,Question,TestPackageId,TestType,Order,Remark")] TestItem testItem)
        {
            if (ModelState.IsValid)
            {
                testItem.TestItemId = Guid.NewGuid();
                _context.Add(testItem);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["TestPackageId"] = new SelectList(GetTestPackages(), "TestPackageId", "Name", testItem.TestPackageId);
            ViewData["TestTypes"] = new SelectList(Enum.GetValues(typeof(TestType)).Cast<TestType>());
            return View(testItem);
        }

        // GET: TestItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testItem = await _context.TestItems.SingleOrDefaultAsync(m => m.TestItemId == id);
            if (testItem == null)
            {
                return NotFound();
            }
            
            ViewData["TestPackageId"] = new SelectList(GetTestPackages(), "TestPackageId", "Name", testItem.TestPackageId);
            ViewData["TestTypes"] = new SelectList(Enum.GetValues(typeof(TestType)).Cast<TestType>());
            return View(testItem);
        }

        // POST: TestItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TestItemId,Answer,Question,TestPackageId,TestType,Order,Remark")] TestItem testItem)
        {
            if (id != testItem.TestItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestItemExists(testItem.TestItemId))
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
            ViewData["TestPackageId"] = new SelectList(GetTestPackages(), "TestPackageId", "Name", testItem.TestPackageId);
            ViewData["TestTypes"] = new SelectList(Enum.GetValues(typeof(TestType)).Cast<TestType>());
            return View(testItem);
        }

        private IQueryable<TestPackage> GetTestPackages()
        {
            Guid? userId = Helper.GetCurrentUserId(User);
            if (userId.HasValue)
            {
                return _context.TestPackages.Where(p => p.UserId == userId.Value);
            }

            return null;
        }

        // GET: TestItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testItem = await _context.TestItems.SingleOrDefaultAsync(m => m.TestItemId == id);
            if (testItem == null)
            {
                return NotFound();
            }

            return View(testItem);
        }

        // POST: TestItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var testItem = await _context.TestItems.SingleOrDefaultAsync(m => m.TestItemId == id);
            _context.TestItems.Remove(testItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TestItemExists(Guid id)
        {
            return _context.TestItems.Any(e => e.TestItemId == id);
        }

    }
}
