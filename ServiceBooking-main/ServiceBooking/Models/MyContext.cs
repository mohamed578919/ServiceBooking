using Microsoft.EntityFrameworkCore;
using ServiceBooking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace ApiDay1.Models
{
    public class MyContext :IdentityDbContext<ApplicationUser>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Application> Applications { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Service> Services { get; set; }


    }
}
