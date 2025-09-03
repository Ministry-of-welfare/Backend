using Microsoft.EntityFrameworkCore;

namespace Dal.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // כאן רושמים את כל ה־Entities כטבלאות
        public DbSet<DalEnvironment> Environments { get; set; }
    }
}
