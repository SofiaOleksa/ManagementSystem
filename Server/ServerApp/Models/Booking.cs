using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ServerApp.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int CoachId { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public string ClientName { get; set; } = null!;

        public bool Status { get; set; }

        // Навігаційні властивості: НЕ валідовувати, можуть бути null
        [ValidateNever]
        public Coach? Coach { get; set; }

        [ValidateNever]
        public TrainingClass? Class { get; set; }
    }
}
