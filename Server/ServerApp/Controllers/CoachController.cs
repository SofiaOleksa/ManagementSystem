// Controllers/CoachController.cs
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace ServerApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CoachController : Controller
    {
        private readonly AppDbContext _context;

        public CoachController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coach
        public async Task<IActionResult> Index()
        {
            var coaches = await _context.Coaches.ToListAsync();
            return View(coaches);
        }

        // GET: Coach/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(m => m.Id == id);

            if (coach == null)
                return NotFound();

            return View(coach);
        }

        // GET: Coach/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coach/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Coach coach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coach);
        }

        // GET: Coach/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var coach = await _context.Coaches.FindAsync(id);
            if (coach == null)
                return NotFound();

            return View(coach);
        }

        // POST: Coach/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Coach coach)
        {
            if (id != coach.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoachExists(coach.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coach);
        }

        // GET: Coach/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var coach = await _context.Coaches
                .FirstOrDefaultAsync(m => m.Id == id);

            if (coach == null)
                return NotFound();

            return View(coach);
        }

        // POST: Coach/Delete/5
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

        private bool CoachExists(int id)
        {
            return _context.Coaches.Any(e => e.Id == id);
        }
    }
}
