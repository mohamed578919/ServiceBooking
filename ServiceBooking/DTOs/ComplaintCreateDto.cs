using ServiceBooking.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DTOs
{
    public class ComplaintCreateDto
    {
        [Required, MaxLength(180)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Description { get; set; } = null!;

        public ComplaintSeverity Severity { get; set; } = ComplaintSeverity.Medium;

        [Required]
        public string AgainstUserId { get; set; } = null!;

        public int? RelatedRequestId { get; set; }
        public int? RelatedServiceId { get; set; }
    }
}
