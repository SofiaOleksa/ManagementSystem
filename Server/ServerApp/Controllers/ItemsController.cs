using ASP_proj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/classes")]
    public class ClassesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassesApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            var classes = await _context.Classes
                .Include(c => c.Coach)   // якщо хочеш одразу бачити тренера
                .ToListAsync();

            return Ok(classes);
        }
    }
}
