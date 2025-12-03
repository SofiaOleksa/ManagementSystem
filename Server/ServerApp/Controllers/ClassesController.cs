using ASP_proj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServerApp.Models;
using System.Security.Claims;

namespace ServerApp.Controllers
{
    [Authorize]
    public class ClassesController : Controller
    {
        private readonly AppDbContext _context;

        public ClassesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            // Підтягуємо тренера для відображення в таблиці
            var classes = await _context.Classes
                .Include(c => c.Coach)
                .ToListAsync();

            return View(classes);
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var classEntity = await _context.Classes
                .Include(c => c.Coach)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classEntity == null) return NotFound();

            return View(classEntity);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name");
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class classEntity)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name", classEntity.CoachId);
                return View(classEntity);
            }

            _context.Add(classEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity == null) return NotFound();

            ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name", classEntity.CoachId);

            return View(classEntity);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Class classEntity)
        {
            if (id != classEntity.ClassId) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["CoachId"] = new SelectList(_context.Coaches, "CoachId", "Name", classEntity.CoachId);
                return View(classEntity);
            }

            _context.Update(classEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var classEntity = await _context.Classes
                .Include(c => c.Coach)
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classEntity == null) return NotFound();

            return View(classEntity);
        }

        // POST: Classes/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classEntity = await _context.Classes.FindAsync(id);
            if (classEntity != null)
            {
                _context.Classes.Remove(classEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
