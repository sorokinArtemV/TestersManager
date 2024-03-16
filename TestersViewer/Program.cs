using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TestersDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IDevStreamsService, DevStreamsService>();
builder.Services.AddScoped<ITestersService, TestersService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();