using Microsoft.EntityFrameworkCore;

namespace Dal.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ��� ������ �� �� ��Entities �������
        public DbSet<DalEnvironment> Environments { get; set; }
    }
}
