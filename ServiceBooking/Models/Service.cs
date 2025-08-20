using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceBooking.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Name { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? HourlyRate { get; set; }
        public int? ExperienceYears { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        [ForeignKey("Provider")]
        public string ProviderId { get; set; } = null!;
        public ApplicationUser Provider { get; set; } = null!;


    }
}
