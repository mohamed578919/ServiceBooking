using Microsoft.AspNetCore.Identity;

namespace ServiceBooking.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Role { get; set; }
        public string? Craft { get; set; }
        public string? VerificationCode { get; set; }
        public bool IsVerified { get; set; } = false;

        public string FullName { get; set; }
        public string? NationalIdImage { get; set; }

    }
}
