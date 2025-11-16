// Controllers/BookingController.cs
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
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // ---------- MVC ЧАСТИНА (адмін-панель) ----------

        // GET: Booking
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Coach)
                .Include(b => b.Class);
            return View(await bookings.ToListAsync());
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Coach)
                .Include(b => b.Class)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public IActionResult Create()
        {
            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name");
            ViewData["TrainingClassId"] = new SelectList(_context.Classes, "Id", "Name");
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name", booking.CoachId);
            ViewData["TrainingClassId"] = new SelectList(_context.Classes, "Id", "Name", booking.ClassId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name", booking.CoachId);
            ViewData["TrainingClassId"] = new SelectList(_context.Classes, "Id", "Name", booking.ClassId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CoachId"] = new SelectList(_context.Coaches, "Id", "Name", booking.CoachId);
            ViewData["TrainingClassId"] = new SelectList(_context.Classes, "Id", "Name", booking.ClassId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Coach)
                .Include(b => b.Class)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ---------- REST API ЧАСТИНА (завдання: POST /api/bookings) ----------

        // POST: /api/bookings
        [HttpPost("/api/bookings")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<object>> CreateBookingApi([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coachExists = await _context.Coaches.AnyAsync(c => c.Id == booking.CoachId);
            if (!coachExists)
                ModelState.AddModelError(nameof(booking.CoachId), "Вказаний тренер не існує.");

            var classExists = await _context.Classes.AnyAsync(c => c.Id == booking.ClassId);
            if (!classExists)
                ModelState.AddModelError(nameof(booking.ClassId), "Вказаний клас не існує.");

            if (string.IsNullOrWhiteSpace(booking.ClientName))
                ModelState.AddModelError(nameof(booking.ClientName), "Ім'я клієнта є обов'язковим.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var result = new
            {
                booking.Id,
                booking.CoachId,
                booking.ClassId,
                booking.ClientName,
                booking.Status
            };

            return Created("/api/bookings/" + booking.Id, result);
        }




    }
}
