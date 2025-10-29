using LoginData.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginData;

public class LoginDbContext(DbContextOptions<LoginDbContext> options) : DbContext(options)
{
    public DbSet<LoginLog> LoginLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoginLog>()
            .HasIndex(l => l.CorrelationId)
            .IsUnique();
    }
}
