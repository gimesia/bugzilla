using Microsoft.EntityFrameworkCore;

namespace bugzilla.Models
{
    public class BugzillaDbContext : DbContext
    {
        public BugzillaDbContext(DbContextOptions<BugzillaDbContext> options):base(options)
        {
                
        }
        
        private DbSet<Developer> Developers { get; set; }
        private DbSet<Role> Roles { get; set; }
        private DbSet<Bug> Bugs { get; set; }
        private DbSet<Fix> Fixes { get; set; }
        private DbSet<Review> Reviews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3306;database=bugzilla;user=root;password=helloworld");
        }

        /* 
           protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               base.OnModelCreating(modelBuilder);
    
               modelBuilder.Entity<Developer>(entity =>
               {
                   entity.HasKey(e => e.Id);
                   entity.Property(e => e.Name).IsRequired();
                   entity.Property(e => e.RoleId).IsRequired();
               });
    
               modelBuilder.Entity<Role>(entity =>
               {
                   entity.HasKey(e => e.Id);
                   entity.Property(e => e.Name).IsRequired();
               });    
               
               modelBuilder.Entity<Bug>(entity =>
               {
                   entity.HasKey(e => e.Id);
                   entity.Property(e => e.DevId).IsRequired();
                   entity.Property(e => e.Closed);
               }); 
               
               modelBuilder.Entity<Fix>(entity =>
               {
                   entity.HasKey(e => e.Id);
                   entity.Property(e => e.DevId).IsRequired();
                   entity.Property(e => e.BugId).IsRequired();
               });
               
               modelBuilder.Entity<Review>(entity =>
               {
                   entity.HasKey(e => e.Id);
                   entity.Property(e => e.DevId).IsRequired();
                   entity.Property(e => e.FixId).IsRequired();
                   entity.Property(e => e.Approved).IsRequired();
               });
           }*/
    }
}