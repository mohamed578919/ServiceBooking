namespace ServiceBooking.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        // Foreign Key للكيلنت
        public string ClientId { get; set; }

        // Navigation Property للعميل
        public ApplicationUser Client { get; set; }

        // Foreign Key للخدمة المطلوبة
        public int ServiceId { get; set; }

        // Navigation Property للخدمة
        public Service Service { get; set; }

        // بيانات إضافية للطلب
        public DateTime RequestDate { get; set; } = DateTime.Now;

        public string? Status { get; set; }  // مثلاً: Pending, Approved, Rejected

        public string? Notes { get; set; }

       
        public ICollection<Application>? Applications { get; set; }
    }
}
