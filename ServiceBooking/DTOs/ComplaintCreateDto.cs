using ServiceBooking.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.DTOs
{
    public class ComplaintCreateDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string AgainstUserId { get; set; } = null!;
        public int? RelatedRequestId { get; set; }
        public int? RelatedServiceId { get; set; }
    }
}
