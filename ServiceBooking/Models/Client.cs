namespace ServiceBooking.Models
{
    public class Client : User
    {
        public ICollection<Request>? Requests { get; set; }
    }
}
