using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // <-- тільки авторизовані користувачі з токеном мають доступ
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return Ok("✅ Вітаю! Ви зайшли в адмін-панель.");
        }
    }
}
