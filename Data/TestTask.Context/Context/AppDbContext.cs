using Microsoft.EntityFrameworkCore;
using TestTask.Context.Entities;

namespace TestTask.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Unit> Units { get; set; }
    }
}
