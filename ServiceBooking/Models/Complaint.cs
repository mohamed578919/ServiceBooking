
using System.ComponentModel.DataAnnotations;
using ServiceBooking.Enums;

namespace ServiceBooking.Models
{
    public class Complaint
    {
        public int Id { get; set; }

        [Required, MaxLength(180)]
        public string Title { get; set; } = null!;

        [Required, MaxLength(4000)]
        public string Description { get; set; } = null!;

        // ممكن تخزّن اسم ملف أو مسار
        [MaxLength(500)]
        public string? AttachmentPath { get; set; }

        public ComplaintSeverity Severity { get; set; } = ComplaintSeverity.Medium;
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Open;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAtUtc { get; set; }

        [MaxLength(2000)]
        public string? AdminNotes { get; set; }

        // علاقات واضحة لتفادي أخطاء EF
        [Required]
        public string FiledByUserId { get; set; } = null!;
        public ApplicationUser FiledByUser { get; set; } = null!;

        [Required]
        public string AgainstUserId { get; set; } = null!;
        public ApplicationUser AgainstUser { get; set; } = null!;

        public string ClientId { get; set; }
        public string ProviderId { get; set; }
        public ApplicationUser Client { get; set; }
        public ApplicationUser Provider { get; set; }

        // علاقات اختيارية لربط الشكوى بطلب/خدمة
        public int? RelatedRequestId { get; set; }
        public int? RelatedServiceId { get; set; }
    }
}
