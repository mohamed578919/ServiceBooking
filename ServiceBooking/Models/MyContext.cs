using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.Models;

namespace ApiDay1.Models
{
    public class MyContext : IdentityDbContext<ApplicationUser>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        //public DbSet<Client> Clients { get; set; }
        //public DbSet<Provider> Providers { get; set; }
        public DbSet<Application> Applications { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Service ↔ Provider
            builder.Entity<Service>()
                   .HasOne(s => s.Provider)
                   .WithMany(u => u.Services)
                   .HasForeignKey(s => s.ProviderId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Complaint ↔ User
            builder.Entity<Complaint>(entity =>
            {
                entity.HasOne(c => c.FiledByUser)
                      .WithMany(u => u.ComplaintsFiled)
                      .HasForeignKey(c => c.FiledByUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.AgainstUser)
                      .WithMany(u => u.ComplaintsAgainst)
                      .HasForeignKey(c => c.AgainstUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Application ↔ Request
            builder.Entity<Application>()
                .HasOne(a => a.Request)
                .WithMany(r => r.Applications)
                .HasForeignKey(a => a.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            // Application ↔ Provider (User)
            builder.Entity<Application>()
            .HasOne(a => a.Provider)
            .WithMany(p => p.Applications)
            .HasForeignKey(a => a.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

            // Request -> Client (ApplicationUser)
            builder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint ↔ Client
            builder.Entity<Complaint>()
                .HasOne(c => c.Client)
                .WithMany()
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint ↔ Provider
            builder.Entity<Complaint>()
                .HasOne(c => c.Provider)
                .WithMany()
                .HasForeignKey(c => c.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
