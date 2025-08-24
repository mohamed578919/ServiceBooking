using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ServiceBooking.Models
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? Bio { get; set; }
        [Precision(18, 2)]
        public double Rating { get; set; }

        public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public virtual ICollection<Complaint>? Complaints { get; set; } = new HashSet<Complaint>();



    }

}
