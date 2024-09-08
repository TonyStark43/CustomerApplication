using CustomerApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static CustomerApplication.Models.MasterDataModels;

namespace CustomerApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // DbSet for Customer_Info
        public DbSet<Customer_Info> CustomerInfos { get; set; }
        public DbSet<GenderMaster> GenderMaster { get; set; }
        public DbSet<DistrictMaster> DistrictMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }

        // Override OnModelCreating if needed to configure entity relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //initial data seeding for StateMaster
            modelBuilder.Entity<StateMaster>().HasData(
                new StateMaster { Id = 1, Name = "Haryana" },
                new StateMaster { Id = 2, Name = "Rajasthan" },
                new StateMaster { Id = 3, Name = "Punjab" }
            );

            // Seed data for GenderMaster
            modelBuilder.Entity<GenderMaster>().HasData(
                new GenderMaster { Id = 1, GenderName = "Male" },
                new GenderMaster { Id = 2, GenderName = "Female" },
                new GenderMaster { Id = 3, GenderName = "Other" }
            );

            // Seed data for DistrictMaster
            modelBuilder.Entity<DistrictMaster>().HasData(
                new DistrictMaster { Id = 1, Name = "Gurgaon", StateId = 1 },
                new DistrictMaster { Id = 2, Name = "Faridabad", StateId = 1 },
                new DistrictMaster { Id = 3, Name = "Hisar", StateId = 1 },
                new DistrictMaster { Id = 4, Name = "Alwar", StateId = 2 },
                new DistrictMaster { Id = 5, Name = "Jaipur", StateId = 2 },
                new DistrictMaster { Id = 6, Name = "Udaipur", StateId = 2 },
                new DistrictMaster { Id = 7, Name = "Chandigarh", StateId = 3 },
                new DistrictMaster { Id = 8, Name = "Amritsar", StateId = 3 }
            );



            // StateMaster - DistrictMaster
            modelBuilder.Entity<DistrictMaster>()
                .HasOne(d => d.State)
                .WithMany(s => s.Districts)
                .HasForeignKey(d => d.StateId)
                .OnDelete(DeleteBehavior.Restrict); 

            // DistrictMaster - Customer_Info
            modelBuilder.Entity<Customer_Info>()
                .HasOne(c => c.District)
                .WithMany(d => d.Customers)
                .HasForeignKey(c => c.DistrictId)
                .OnDelete(DeleteBehavior.Restrict); 

            // GenderMaster - Customer_Info
            modelBuilder.Entity<Customer_Info>()
                .HasOne(c => c.Gender)
                .WithMany(g => g.Customers)
                .HasForeignKey(c => c.GenderId)
                .OnDelete(DeleteBehavior.Restrict); 
        }

    }
}
