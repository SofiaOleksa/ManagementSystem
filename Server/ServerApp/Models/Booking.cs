using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ServerApp.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int CoachId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public string ClientName { get; set; } = null!;

        // "Pending", "Confirmed", "Cancelled"
        [Required]
        public string Status { get; set; } = "Pending";

        [ValidateNever]
        public Coach? Coach { get; set; }

        [ValidateNever]
        public Class? Class { get; set; }
    }
}
