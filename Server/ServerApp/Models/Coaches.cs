using ServerApp.Models;
using System.Security.Claims;

namespace ServerApp.Models
{
    public class Coach
    {
        public int CoachId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // Тренер може мати багато занять (Classes)
        public ICollection<Classes> Classes { get; set; } = new List<Classes>();

        // І багато бронювань (Bookings), якщо хочеш тримати зв’язок і тут
        public ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
    }
}
