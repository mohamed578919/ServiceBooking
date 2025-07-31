namespace ServiceBooking.Models
{
    public class Provider : User
    {
        public string? Bio { get; set; }
        public string? Specialization { get; set; }
        public double Rating { get; set; }

        public ICollection<Service>? Services { get; set; }
    }

}
