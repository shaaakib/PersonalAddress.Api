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
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure One-to-One Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile) 
                .WithOne(p => p.User)   
                .HasForeignKey<Profile>(p => p.UserId);

            // Configure One-to-Many Relationship
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithOne(c => c.Student)
                .HasForeignKey(c => c.StudentId);

        }
    }
}
