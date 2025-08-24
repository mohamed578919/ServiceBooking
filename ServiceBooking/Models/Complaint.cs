
using System.ComponentModel.DataAnnotations;
using ServiceBooking.Enums;

namespace ServiceBooking.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        // مين اللي اشتكى (Client أو Provider)
       
        // بيانات الشكوى
        public string Title { get; set; }
        public string Description { get; set; }
        public ComplaintSeverity Severity { get; set; } = ComplaintSeverity.Medium;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Resolved { get; set; } = false;

        //================================================
        public int RequestId { get; set; }
        public Request Request { get; set; } = default!;


        //================================================

        public int ClientId { get; set; }

        // Navigation Property للعميل
        public Client Client { get; set; }

        //=======================================


        public int ProviderId { get; set; }
        public Provider Provider { get; set; } = default!;
    }
}
