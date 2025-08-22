using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskHive.Models
{
    public class AuthDbContext : IdentityDbContext<Users>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Users>()
                   .HasIndex(u => u.Email)
                   .IsUnique();
        }
    }
}
