namespace ServiceBooking.DTOs
{
    public class SendMessageDto
    {
        public int RequestId { get; set; }
        public string Text { get; set; } = default!;
    }
}
