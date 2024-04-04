using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestersManager.Core.Domain.IdentityEntities;
using TestersManager.Core.Domain.RepositoryContracts;
using TestersManager.Core.ServiceContracts;
using TestersManager.Core.Services;
using TestersManager.Infrastructure.DbContext;
using TestersManager.Infrastructure.Repositories;
using TestersManager.UI.Filters.ActionFilters;

namespace TestersManager.UI.StartupExtensions;

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

            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

            options.AddPolicy("NotAuthorized", policy =>
            {
                policy.RequireAssertion( context => context.User.Identity?.IsAuthenticated == false);
            });
        });

        services.ConfigureApplicationCookie(options => { options.LoginPath = "/Account/Login"; });

        services.AddHttpLogging(logging =>
        {
            logging.LoggingFields =
                HttpLoggingFields.RequestPropertiesAndHeaders |
                HttpLoggingFields.RequestPropertiesAndHeaders;
        });

        services.AddScoped<IDevStreamsGetterService, DevStreamsGetterService>();
        services.AddScoped<IDevStreamsAdderService, DevStreamsAdderService>();
        services.AddScoped<ITestersGetterService, TestersGetterService>();
        services.AddScoped<ITestersAdderService, TestersAdderService>();
        services.AddScoped<ITestersSorterService, TestersSorterService>();
        services.AddScoped<ITestersDeleterService, TestersDeleterService>();
        services.AddScoped<ITestersUpdaterService, TestersUpdaterService>();

        services.AddScoped<IDevStreamsRepository, DevStreamsRepository>();
        services.AddScoped<ITestersRepository, TestersRepository>();

        return services;
    }
}