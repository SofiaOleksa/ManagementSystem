namespace ServerApp.Models;

public class Coach
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;

    // Один тренер має багато занять
    public ICollection<TrainingClass> Classes { get; set; } = new List<TrainingClass>();

    // І багато бронювань (через CoachId у Booking)
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
