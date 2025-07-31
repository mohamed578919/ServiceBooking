namespace ServiceBooking.DTOs
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } // رقم التليفون
        public string Role { get; set; } // "Client" or "Provider"
        public string? Craft { get; set; } // Required if Role == "Provider"
    }
}
