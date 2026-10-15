using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RDsport.Models;

namespace RDsport.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Team> teams { get; set; }
        public DbSet<Player> players { get; set; }
        public DbSet<ApplicationUser> users { get; set; }
    }
}
