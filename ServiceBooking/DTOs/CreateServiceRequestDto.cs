namespace ServiceBooking.DTOs
{
    public class CreateServiceRequestDto
    {
        public string ClientUserNamed { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Service { get; set; }


        public decimal Budget { get; set; }
    }
}
