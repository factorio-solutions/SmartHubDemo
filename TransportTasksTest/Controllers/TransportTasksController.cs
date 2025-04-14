using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TransportTasksTest.Data;
using TransportTasksTest.Models;

namespace TransportTasksTest.Controllers
{
    public class TransportTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransportTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransportTasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.TransportTask.ToListAsync());
        }

        // GET: TransportTasks
        public async Task<IActionResult> Search()
        {
            return View();
        }

        // POST: TransportTasks/Search
        public async Task<IActionResult> SearchResults(string searchPhrase)
        {
            return View("Index", await _context.TransportTask.Where(t => t.Name.Contains(searchPhrase)
                                                                   || t.Description.Contains(searchPhrase))
                                                             .ToListAsync());
        }

        // GET: TransportTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportTask = await _context.TransportTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transportTask == null)
            {
                return NotFound();
            }

            return View(transportTask);
        }

        // GET: TransportTasks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransportTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CreatedAt,UpdatedAt,IsCompleted")] TransportTask transportTask)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transportTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transportTask);
        }

        // GET: TransportTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportTask = await _context.TransportTask.FindAsync(id);
            if (transportTask == null)
            {
                return NotFound();
            }
            return View(transportTask);
        }

        // POST: TransportTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreatedAt,UpdatedAt,IsCompleted")] TransportTask transportTask)
        {
            if (id != transportTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transportTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportTaskExists(transportTask.Id))
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
            return View(transportTask);
        }

        // GET: TransportTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transportTask = await _context.TransportTask
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transportTask == null)
            {
                return NotFound();
            }

            return View(transportTask);
        }

        // POST: TransportTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transportTask = await _context.TransportTask.FindAsync(id);
            if (transportTask != null)
            {
                _context.TransportTask.Remove(transportTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportTaskExists(int id)
        {
            return _context.TransportTask.Any(e => e.Id == id);
        }
    }
}
