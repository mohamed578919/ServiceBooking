namespace ServiceBooking.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string ProviderId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? HourlyRate { get; set; }
        public int? ExperienceYears { get; set; }
        public bool IsActive { get; set; }
    }

}
