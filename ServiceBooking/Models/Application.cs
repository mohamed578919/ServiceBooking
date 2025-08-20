namespace ServiceBooking.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } = "Pending";

        public int RequestId { get; set; }
        public Request Request { get; set; }

        public string ProviderId { get; set; }
        public Provider Provider { get; set; }   // ✅

        public string Message { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}
