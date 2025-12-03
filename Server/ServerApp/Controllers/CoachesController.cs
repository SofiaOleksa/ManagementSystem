using ASP_proj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    public class CoachesController : Controller
    {
        private readonly AppDbContext _context;

        public CoachesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coaches
        public async Task<IActionResult> Index()
        {
            var coaches = await _context.Coaches.ToListAsync();
            return View(coaches);
        }

        // GET: Coaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(c => c.CoachId == id);

            if (coach == null) return NotFound();

            return View(coach);
        }

        // GET: Coaches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coaches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coaches coach)
        {
            if (!ModelState.IsValid)
                return View(coach);

            _context.Add(coach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Coaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null) return NotFound();

            return View(coach);
        }

        // POST: Coaches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coaches coach)
        {
            if (id != coach.CoachId) return NotFound();

            if (!ModelState.IsValid)
                return View(coach);

            _context.Update(coach);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Coaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(c => c.CoachId == id);

            if (coach == null) return NotFound();

            return View(coach);
        }

        // POST: Coaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coach = await _context.Coaches.FindAsync(id);
            if (coach != null)
            {
                _context.Coaches.Remove(coach);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
