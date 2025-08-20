using Microsoft.AspNetCore.Identity;

namespace ServiceBooking.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public string? Craft { get; set; }
        public string Status { get; set; } = "Pending"; // Active, Suspended, Deleted
        public string? VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;

<<<<<<< HEAD
        // ✅ Service navigation واحدة بس
        public ICollection<Service> Services { get; set; } = new List<Service>();

        // ✅ Complaints navigation واضحة
        public ICollection<Complaint> ComplaintsFiled { get; set; } = new List<Complaint>();
        public ICollection<Complaint> ComplaintsAgainst { get; set; } = new List<Complaint>();

        // Requests submitted by this client
        public ICollection<Request>? Requests { get; set; }
=======
        public string FullName { get; set; }
        public string? NationalIdImage { get; set; }
>>>>>>> bca8e2c23ae9631e32ba7ebf9db1ee2c3a93e3cd

    }
}
