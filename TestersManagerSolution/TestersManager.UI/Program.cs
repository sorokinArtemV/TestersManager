using Serilog;
using TestersManager.UI.Middleware;
using TestersManager.UI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

// first
app.UseHsts();

// then after
app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseStaticFiles();

// Identify method based route
app.UseRouting();

// Reading identity cookie
app.UseAuthentication();

app.UseAuthorization();

// Actions + filters
app.MapControllers();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    // Admin/Home/Index
    endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();


// makes program class available for testing -
namespace TestersManager.UI
{
    public partial class Program
    {
    }
}