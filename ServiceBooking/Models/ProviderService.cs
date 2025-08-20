namespace ServiceBooking.Models
{
    public class ProviderService
    {
        public int Id { get; set; }
        public string ProviderId { get; set; }
        public int ServiceId { get; set; }

        // Navigation
        public ApplicationUser Provider { get; set; }
        public Service Service { get; set; }
    }
}
