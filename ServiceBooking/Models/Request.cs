namespace ServiceBooking.Models
{
    public class Request
    {
        public int Id { get; set; }

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
    }
}
