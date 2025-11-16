// Controllers/TrainingClassController.cs
using System.Collections.Generic;                  // ⬅️ додай це
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ServerApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainingClassController : Controller
    {
        private readonly AppDbContext _context;

        public TrainingClassController(AppDbContext context)
        {
            _context = context;
        }

        // ---------- MVC ЧАСТИНА (адмін-панель) ----------

        // GET: TrainingClass
        public async Task<IActionResult> Index()
        {
            var classes = _context.Classes
                .Include(c => c.Coach);
            return View(await classes.ToListAsync());
        }

        // GET: TrainingClass/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingClass = await _context.Classes
                .Include(c => c.Coach)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingClass == null)
                return NotFound();

            return View(trainingClass);
        }

        // GET: TrainingClass/Create
        public IActionResult Create()
        {
            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name");
            return View();
        }

        // POST: TrainingClass/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingClass trainingClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name", trainingClass.CoachId);
            return View(trainingClass);
        }

        // GET: TrainingClass/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingClass = await _context.Classes.FindAsync(id);
            if (trainingClass == null)
                return NotFound();

            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name", trainingClass.CoachId);
            return View(trainingClass);
        }

        // POST: TrainingClass/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TrainingClass trainingClass)
        {
            if (id != trainingClass.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trainingClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name", trainingClass.CoachId);
            return View(trainingClass);
        }

        // GET: TrainingClass/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var trainingClass = await _context.Classes
                .Include(c => c.Coach)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (trainingClass == null)
                return NotFound();

            return View(trainingClass);
        }

        // POST: TrainingClass/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingClass = await _context.Classes.FindAsync(id);
            if (trainingClass != null)
            {
                _context.Classes.Remove(trainingClass);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ---------- REST API ЧАСТИНА (завдання 6) ----------

        // GET: /api/classes
        [HttpGet("/api/classes")]
        public async Task<ActionResult<IEnumerable<object>>> GetClassesApi()
        {
            var classes = await _context.Classes
                .Include(c => c.Coach)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.TimeSlot,
                    CoachId = c.CoachId,
                    CoachName = c.Coach.Name
                })
                .ToListAsync();

            return Ok(classes);
        }

    }
}
