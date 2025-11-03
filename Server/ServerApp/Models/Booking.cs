namespace ServerApp.Models;

public class Booking
{
    public int Id { get; set; }

    public int CoachId { get; set; }
    public Coach Coach { get; set; } = null!;

    public int ClassId { get; set; }
    public TrainingClass Class { get; set; } = null!;

    public string ClientName { get; set; } = null!;
    public bool Status { get; set; }
}
