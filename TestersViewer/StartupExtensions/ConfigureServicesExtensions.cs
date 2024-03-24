using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using TestersViewer.Filters.ActionFilters;

namespace TestersViewer.StartupExtensions;

public static class ConfigureServicesExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ResponseHeaderActionFilter>();

        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new ResponseHeaderActionFilter(logger)
                { Key = "X-Custom-Key-Global", Value = "X-Custom-Value-Global", Order = 2 });
        });

        services.AddDbContext<ApplicatonDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields =
                HttpLoggingFields.RequestPropertiesAndHeaders |
                HttpLoggingFields.RequestPropertiesAndHeaders;
        });

        services.AddScoped<IDevStreamsService, DevStreamsService>();
        services.AddScoped<ITestersService, TestersService>();

        services.AddScoped<IDevStreamsRepository, DevStreamsRepository>();
        services.AddScoped<ITestersRepository, TestersRepository>();

        return services;
    }
}