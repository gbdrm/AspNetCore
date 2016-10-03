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
    public class TestOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestOptionsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: TestOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TestOptions.Include(t => t.TestItem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TestOptions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testOption = await _context.TestOptions.SingleOrDefaultAsync(m => m.TestOptionId == id);
            if (testOption == null)
            {
                return NotFound();
            }

            return View(testOption);
        }

        // GET: TestOptions/Create
        public IActionResult Create()
        {
            ViewData["TestItemId"] = new SelectList(_context.TestItems, "TestItemId", "TestItemId");
            return View();
        }

        // POST: TestOptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestOptionId,Content,TestItemId")] TestOption testOption)
        {
            if (ModelState.IsValid)
            {
                testOption.TestOptionId = Guid.NewGuid();
                _context.Add(testOption);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["TestItemId"] = new SelectList(_context.TestItems, "TestItemId", "TestItemId", testOption.TestItemId);
            return View(testOption);
        }

        // GET: TestOptions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testOption = await _context.TestOptions.SingleOrDefaultAsync(m => m.TestOptionId == id);
            if (testOption == null)
            {
                return NotFound();
            }
            ViewData["TestItemId"] = new SelectList(_context.TestItems, "TestItemId", "TestItemId", testOption.TestItemId);
            return View(testOption);
        }

        // POST: TestOptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TestOptionId,Content,TestItemId")] TestOption testOption)
        {
            if (id != testOption.TestOptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestOptionExists(testOption.TestOptionId))
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
            ViewData["TestItemId"] = new SelectList(_context.TestItems, "TestItemId", "TestItemId", testOption.TestItemId);
            return View(testOption);
        }

        // GET: TestOptions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testOption = await _context.TestOptions.SingleOrDefaultAsync(m => m.TestOptionId == id);
            if (testOption == null)
            {
                return NotFound();
            }

            return View(testOption);
        }

        // POST: TestOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var testOption = await _context.TestOptions.SingleOrDefaultAsync(m => m.TestOptionId == id);
            _context.TestOptions.Remove(testOption);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TestOptionExists(Guid id)
        {
            return _context.TestOptions.Any(e => e.TestOptionId == id);
        }
    }
}
