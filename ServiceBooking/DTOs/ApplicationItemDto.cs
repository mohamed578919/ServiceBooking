using ServiceBooking.Enums;

namespace ServiceBooking.DTOs
{
    public class ApplicationItemDto
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string ProviderName { get; set; } = default!;
        public decimal? BidAmount { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
