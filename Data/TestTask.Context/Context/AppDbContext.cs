using Microsoft.EntityFrameworkCore;
using TestTask.Context.Entities;

namespace TestTask.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Unit>().ToTable("Units"); // Assuming your table is named "Units"
            base.OnModelCreating(modelBuilder);
        }
    }
}