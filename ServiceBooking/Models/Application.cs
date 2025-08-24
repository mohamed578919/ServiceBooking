using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceBooking.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } = "Pending";

        public string phoneNumber {  get; set; }


        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        [Precision(18, 2)]
        public decimal ProposedPrice { get; set; }

      
        public DateTime? UpdatedAt { get; set; }



       
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;


        //============================================================
        public int ProviderId { get; set; }
        public Provider Provider { get; set; } = default!;

        public int RequestId { get; set; }
        public Request Request { get; set; } = default!;
    }
}
