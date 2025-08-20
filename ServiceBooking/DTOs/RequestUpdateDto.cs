namespace ServiceBooking.DTOs
{
    public class RequestUpdateDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int ServiceId { get; set; }
        public string? Notes { get; set; }
        public DateTime? PreferredDate { get; set; }
        public string? Status { get; set; }
    }
}
