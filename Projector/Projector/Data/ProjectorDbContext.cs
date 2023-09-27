using Microsoft.EntityFrameworkCore;

namespace Projector.Data
{
    public class ProjectorDbContext : DbContext
    {
        /* CONSTRUCTORS */
        public ProjectorDbContext() { }

        public ProjectorDbContext(DbContextOptions<ProjectorDbContext> options) : base(options) {
        
        }

        /* PROPERTIES */
        public DbSet<User> Users { get; set; }

        /* METHODS */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=Projector;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
