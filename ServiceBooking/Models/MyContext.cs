using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceBooking.Models;

namespace ApiDay1.Models
{
    public class MyContext : IdentityDbContext<ApplicationUser>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Provider> Providers { get; set; }
        //public DbSet<ProviderSkill> ProviderSkills => Set<ProviderSkill>();
        //public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();
        //public DbSet<ProviderApplication> ProviderApplications => Set<ProviderApplication>();
        //public DbSet<Conversation> Conversations => Set<Conversation>();
        //public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
        public DbSet<Application> Applications { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<Service> Services { get; set; }

        //public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Request → Client (One-to-Many) 
            builder.Entity<Request>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Requests)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Request → Service (One-to-Many)
            builder.Entity<Request>()
                .HasOne(r => r.Service)
                .WithMany(s => s.Requests)
                .HasForeignKey(r => r.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Application → Request (One-to-Many)
            builder.Entity<Application>()
                .HasOne(a => a.Request)
                .WithMany(r => r.Applications)
                .HasForeignKey(a => a.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Application → Provider (One-to-Many)
            builder.Entity<Application>()
                .HasOne(a => a.Provider)
                .WithMany(p => p.Applications)
                .HasForeignKey(a => a.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);


            // Complaint → Client
            builder.Entity<Complaint>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Complaints)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint → Provider
            builder.Entity<Complaint>()
                .HasOne(c => c.Provider)
                .WithMany(p => p.Complaints)
                .HasForeignKey(c => c.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint → Request 
            builder.Entity<Complaint>()
                .HasOne(c => c.Request)
                .WithMany(r => r.Complaints)
                .HasForeignKey(c => c.RequestId)
                .OnDelete(DeleteBehavior.Restrict);


            //===========================================================
            // ✅ Seeding for Service
            builder.Entity<Service>().HasData(
               
                new Service
                {
                    Id = 1,
                    Name = "سباكه",
                    Description = "تصليح حنافيات مواسير كل ما يخص السباكه",
                    HourlyRate = 100,
                    ExperienceYears = 5,
                    IsActive = true
                },
                new Service
                {
                    Id = 2,
                    Name = "نجاره",
                    Description = "تركيب ابواب اعاده وشبابيك وكل ما يخص النجاره",
                    HourlyRate = 120,
                    ExperienceYears = 7,
                    IsActive = true
                },
                new Service
                {
                    Id = 4,
                    Name = "كهرباء",
                    Description = "كل ما يخص الكهرباء",
                    HourlyRate = 150,
                    ExperienceYears = 6,
                    IsActive = true
                },
                 new Service
                 {
                     Id = 5,
                     Name = "نقاشه",
                     Description = "دهان جميع الحوائط وجميع ما يخص النقاشه",
                     HourlyRate = 100,
                     ExperienceYears = 5,
                     IsActive = true
                 },
                  new Service
                  {
                      Id = 6,
                      Name = "خدمات اخرى",
                      Description = "خدمات اخرى",
                      HourlyRate = 100,
                      ExperienceYears = 5,
                      IsActive = true
                  }


            );
        }


    }
}
