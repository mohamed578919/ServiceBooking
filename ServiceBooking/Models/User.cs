using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.Models
{
    public abstract class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string? ProfileImageUrl { get; set; }

        // علاقات مشتركة
    }

}
