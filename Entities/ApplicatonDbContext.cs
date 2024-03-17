using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ApplicatonDbContext : DbContext
{
    public ApplicatonDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<DevStream> DevStreams { get; set; }
    public virtual DbSet<Tester> Testers { get; set; }

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

        // fluent api
        modelBuilder.Entity<Tester>().Property(x => x.MonthsOfWorkExperience)
            .HasColumnName("WorksFor")
            .HasColumnType("INTEGER")
            .HasDefaultValue(1);

        // table relations
        // modelBuilder.Entity<Tester>(entity =>
        // {
        //     entity.HasOne<DevStream>(s => s.DevStream)
        //         .WithMany(t => t.Testers)
        //         .HasForeignKey(t => t.DevStreamId);
        // });
    }
}