using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.Models
{
    public class Complaint
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int? ProviderId { get; set; }
        public Provider? Provider { get; set; }
        public int? RequestId { get; set; }
        public Request? Request { get; set; }
        [Required]
        public string Status { get; set; } = "New"; // new , in-progress , closed
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
