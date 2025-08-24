using ServiceBooking.Enums;

namespace ServiceBooking.DTOs
{
    public class ComplaintDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ComplaintStatus Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public string FiledByUserId { get; set; } = null!;
        public string AgainstUserId { get; set; } = null!;
        public int? RelatedRequestId { get; set; }
        public int? RelatedServiceId { get; set; }
        public string? AdminNotes { get; set; }
    }
}
