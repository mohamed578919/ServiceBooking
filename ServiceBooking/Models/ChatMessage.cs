namespace ServiceBooking.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public Conversation Conversation { get; set; } = default!;
        public string SenderUserId { get; set; } = default!;
        public string Text { get; set; } = default!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
    }
}
