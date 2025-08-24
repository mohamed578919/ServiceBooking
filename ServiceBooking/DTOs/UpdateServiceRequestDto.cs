namespace ServiceBooking.DTOs
{
    public class UpdateServiceRequestDto
    {
        public string ClientUserNamed { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Budget { get; set; }
    }
}
