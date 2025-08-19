namespace ServiceBooking.DTOs
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }   // خلي بالك من الكابيتال N 👈
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string? Craft { get; set; }
        public string Password { get; set; }

        // هنا المشكلة 👇 لازم تبقى IFormFile مش string
        public IFormFile? NationalIdImage { get; set; }// هيخزن URL أو Base64

    }
}
