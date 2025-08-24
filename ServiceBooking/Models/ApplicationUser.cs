using Microsoft.AspNetCore.Identity;

namespace ServiceBooking.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? Full_Name { get; set; }
        public string? Role { get; set; }
        public string? Craft { get; set; }
        public string Status { get; set; } = "Pending"; // Active, Suspended, Deleted
        public string? VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? NationalIdImage { get; set; }

       

        // Requests submitted by this client
        public ICollection<Request>? Requests { get; set; }
        public Client? ClientProfile { get; set; }
        public Provider? ProviderProfile { get; set; }
    }
}
