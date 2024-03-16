using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class TestersDbContext : DbContext
{
    public TestersDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<DevStream> DevStreams { get; set; }
    public DbSet<Tester> Testers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DevStream>().ToTable("DevStreams");
        modelBuilder.Entity<Tester>().ToTable("Testers");

        // Seed data
        var streamsJson = File.ReadAllText("wwwroot/streams.json");
        var devStreams = JsonSerializer.Deserialize<List<DevStream>>(streamsJson);

        foreach (var stream in devStreams!) modelBuilder.Entity<DevStream>().HasData(stream);

        var testersJson = File.ReadAllText("wwwroot/testers.json");
        var testers = JsonSerializer.Deserialize<List<Tester>>(testersJson);

        foreach (var tester in testers!) modelBuilder.Entity<Tester>().HasData(tester);
    }
}