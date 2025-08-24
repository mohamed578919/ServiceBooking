namespace ServiceBooking.Models
{
    public class Conversation
    {
        public int Id { get; set; }

        public int RequestId { get; set; }
        public ServiceRequest Request { get; set; } = default!;

        public int ClientProfileId { get; set; }
        public Client ClientProfile { get; set; } = default!;

        public int ProviderProfileId { get; set; }
        public Provider ProviderProfile { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
