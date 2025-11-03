namespace ServerApp.Models;

public class TrainingClass
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime TimeSlot { get; set; }

    public int CoachId { get; set; }
    public Coach Coach { get; set; } = null!;

    // Одне заняття має багато бронювань
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
