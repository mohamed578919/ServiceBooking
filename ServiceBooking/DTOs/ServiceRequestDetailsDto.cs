namespace ServiceBooking.DTOs
{
    public class ServiceRequestDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Budget { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ClientName { get; set; }

        public List<ApplicationDTO>? Applications { get; set; }
    }
}
