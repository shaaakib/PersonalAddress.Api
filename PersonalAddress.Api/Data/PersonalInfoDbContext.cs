using Microsoft.EntityFrameworkCore;
using PersonalAddress.Api.Models;

namespace PersonalAddress.Api.Data
{
    public class PersonalInfoDbContext : DbContext
    {
        public PersonalInfoDbContext(DbContextOptions<PersonalInfoDbContext> options) : base(options) { }

        public DbSet<PersonalInfo> PersonalInfo { get; set; }
    }
}
