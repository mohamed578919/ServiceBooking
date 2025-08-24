using ServiceBooking.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DTOs
{
    public class ComplaintCreateDto
    {
        public string AgainstUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ComplaintSeverity Severity { get; set; } = ComplaintSeverity.Medium;
    }
}
