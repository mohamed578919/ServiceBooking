using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceBooking.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        [Precision(18, 2)]
        public decimal? HourlyRate { get; set; }
        public int? ExperienceYears { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Request> Requests { get; set; } = new HashSet<Request>();
        public ICollection<Provider> Providers { get; set; } = new HashSet<Provider>();

    }
}
