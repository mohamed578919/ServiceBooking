
using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
