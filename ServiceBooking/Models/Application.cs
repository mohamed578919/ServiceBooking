namespace ServiceBooking.Models
{
    public class Application
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public Request Request { get; set; }

        public string ProviderId { get; set; }  // من جدول Identity Users
        public ApplicationUser Provider { get; set; }

        public string Message { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.Now;
    }
}
