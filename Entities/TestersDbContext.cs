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
       var streamsJson = File.ReadAllText("streams.json");
       var devStreams = JsonSerializer.Deserialize<List<DevStream>>(streamsJson);

       foreach (var stream in devStreams!)
       {
           modelBuilder.Entity<DevStream>().HasData(stream);
       }
       
       var testersJson = File.ReadAllText("testers.json");
       var testers = JsonSerializer.Deserialize<List<DevStream>>(testersJson);

       foreach (var tester in testers!)
       {
           modelBuilder.Entity<DevStream>().HasData(tester);
       }

    }
}