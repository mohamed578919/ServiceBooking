namespace ServiceBooking.Models
{
    public class Provider : User
    {
        public int Id { get; set; }

        public ICollection<Service>? Services { get; set; }
    }
}
