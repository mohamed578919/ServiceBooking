using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DTOs
{
    public class ServiceCreateDto
    {
        [Required, MaxLength(120)]
        public string Name { get; set; } = null!;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Range(0, 1_000_000)]
        public decimal? HourlyRate { get; set; }

        [Range(0, 80)]
        public int? ExperienceYears { get; set; }
    }
}
