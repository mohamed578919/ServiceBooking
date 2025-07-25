namespace ServiceBooking.DTOs
{
    public class CreateComplaintDTO
    {
        public string Description { get; set; }
        public int ClientId { get; set; }
        public int? ProviderId { get; set; }
        public int? RequestId { get; set; }
    }
}
