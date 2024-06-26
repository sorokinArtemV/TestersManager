using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestersManager.Core.Domain.Entities;
using TestersManager.Core.Domain.IdentityEntities;

namespace TestersManager.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<DevStream> DevStreams { get; set; }
    public virtual DbSet<Tester> Testers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // code for compiling ///////////////////////////
        modelBuilder.Entity<IdentityUserLogin<Guid>>().HasNoKey();
        modelBuilder.Entity<IdentityUserRole<Guid>>().HasNoKey();
        modelBuilder.Entity<IdentityUserClaim<Guid>>().HasNoKey();
        modelBuilder.Entity<IdentityUserToken<Guid>>().HasNoKey();
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().HasNoKey();

        base.OnModelCreating(modelBuilder);
        // /////////////////////////////////////////////////


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
    }
}