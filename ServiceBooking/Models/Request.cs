using Microsoft.EntityFrameworkCore;

namespace ServiceBooking.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        [Precision(18, 2)]
        public decimal Budget { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key للكيلنت
        public int ClientId { get; set; }

        // Navigation Property للعميل
        public Client Client { get; set; }

        // Foreign Key للخدمة المطلوبة
        public int ServiceId { get; set; }

        // Navigation Property للخدمة
        public Service Service { get; set; }

        // بيانات إضافية للطلب
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public string? Status { get; set; }  // مثلاً: Pending, Approved, Rejected

        public string? Notes { get; set; }



        //==============================================================

        public PaymentTransaction? PaymentTransaction { get; set; }

        public virtual ICollection<Application>? Applications { get; set; } = new HashSet<Application>();

        public virtual ICollection<Complaint>? Complaints { get; set; } = new HashSet<Complaint>();

    }
}
