using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole().AddDebug();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicatonDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IDevStreamsService, DevStreamsService>();
builder.Services.AddScoped<ITestersService, TestersService>();

builder.Services.AddScoped<IDevStreamsRepository, DevStreamsRepository>();
builder.Services.AddScoped<ITestersRepository, TestersRepository>();

var app = builder.Build();

if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.Logger.LogDebug("debug-message");
app.Logger.LogInformation("info-message");
app.Logger.LogWarning("warning-message");
app.Logger.LogError("error-message");
app.Logger.LogCritical("critical-message");


app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();


// makes program class available for testing 
public partial class Program
{
}