using Microsoft.EntityFrameworkCore;

namespace Entities;

public class TestersDbContext : DbContext
{
    public TestersDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DevStream> DevStreams { get; set; }
    public DbSet<Tester> Testers { get; set; }
}