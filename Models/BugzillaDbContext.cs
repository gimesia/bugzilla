using Microsoft.EntityFrameworkCore;

namespace bugzilla.Models
{
    public class BugzillaDbContext : DbContext
    {
        public BugzillaDbContext(DbContextOptions<BugzillaDbContext> options):base(options)
        {
                
        }
        
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Fix> Fixes { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3306;database=bugzilla;user=root;password=helloworld");
        }
    }
}