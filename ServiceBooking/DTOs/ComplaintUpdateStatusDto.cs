using ServiceBooking.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DTOs
{
    public class ComplaintUpdateStatusDto
    {
        [Required]
        public ComplaintStatus Status { get; set; }
        [MaxLength(2000)]
        public string? AdminNotes { get; set; }
    }
}
