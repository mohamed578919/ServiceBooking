namespace ServiceBooking.DTOs
{
    public class ApplicationDTO
    {
        public int Id { get; set; }

        public decimal ProposedPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RequestId { get; set; }
        public string RequestTitle { get; set; }

        public string PhoneNumber { get; set; }
        public string ProviderName { get; set; }
        public string Message { get; set; }
    }
}
