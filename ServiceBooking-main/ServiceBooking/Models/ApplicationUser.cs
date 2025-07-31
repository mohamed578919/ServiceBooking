using Microsoft.AspNetCore.Identity;

namespace ServiceBooking.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Role { get; set; } 
        public string? Craft { get; set; } 
    }
}
