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


app.UseHttpLogging();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();


// makes program class available for testing 
namespace TestersManager.UI
{
    public partial class Program
    {
    }
}