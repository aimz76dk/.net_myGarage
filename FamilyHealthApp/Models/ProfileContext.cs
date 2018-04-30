using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyHealthApp.Models
{
    public class ProfileContext : IdentityDbContext<ApplicationUser>
    {
        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options)
        {
        }

        // Set models to schemes in DB
        public DbSet<Car> Cars { get; set; }
        public DbSet<Comment> Comments { get; set; }
       
    }
}
