using Microsoft.EntityFrameworkCore;
using PersonalAddress.Api.Models;

namespace PersonalAddress.Api.Data
{
    public class PersonalInfoDbContext : DbContext
    {
        public PersonalInfoDbContext(DbContextOptions<PersonalInfoDbContext> options) : base(options) { }

        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<PersonalInfoAddress> PersonalInfoAddress { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure One-to-One Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile) // User has one Profile
                .WithOne(p => p.User)   // Profile has one User
                .HasForeignKey<Profile>(p => p.UserId); // Foreign Key
        }
    }
}
