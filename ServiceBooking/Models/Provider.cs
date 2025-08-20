namespace ServiceBooking.Models
{
    public class Provider : ApplicationUser
    {
        public string Skill { get; set; }
        public string Bio { get; set; }
        public ICollection<Service> Services { get; set; }
        public ICollection<Application> Applications { get; set; }
    }

}
