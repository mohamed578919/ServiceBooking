using System.ComponentModel.DataAnnotations;

namespace ServiceBooking.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }    

        public ICollection<Provider>? Providers { get; set; }
    }
}
