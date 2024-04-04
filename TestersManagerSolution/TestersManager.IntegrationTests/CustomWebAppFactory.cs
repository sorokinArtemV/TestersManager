using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestersManager.Infrastructure.DbContext;
using TestersManager.UI;

namespace TestersManager.IntegrationsTests;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(x =>
                x.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null) services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
        });
    }
}