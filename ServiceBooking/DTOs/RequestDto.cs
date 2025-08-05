namespace ServiceBooking.DTOs
{
    public class RequestDto
    {
        public int Id { get; set; }

        public string? Notes { get; set; }

        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = string.Empty;

        public DateTime RequestDate { get; set; }
    }

}
