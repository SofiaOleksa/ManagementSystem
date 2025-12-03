using ASP_proj.Models;

namespace ServerApp.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; } = null!;

        // Рекомендовано тримати TimeSlot як string ("Пн 18:00–19:00")
        public string TimeSlot { get; set; } = null!;

        // Зв'язок з тренером
        public int CoachId { get; set; }
        public Coach Coach { get; set; } = null!;

        // Заняття може мати багато бронювань
        public ICollection<Bookings> Bookings { get; set; } = new List<Bookings>();
    }
}
