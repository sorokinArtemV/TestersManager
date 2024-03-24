using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using TestersViewer.Filters.ActionFilters;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.AddTransient<ResponseHeaderActionFilter>();

var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ResponseHeaderActionFilter(logger)
        { Key = "X-Custom-Key-Global", Value = "X-Custom-Value-Global", Order = 2 });
});

builder.Services.AddDbContext<ApplicatonDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields =
        HttpLoggingFields.RequestPropertiesAndHeaders |
        HttpLoggingFields.RequestPropertiesAndHeaders;
});

builder.Services.AddScoped<IDevStreamsService, DevStreamsService>();
builder.Services.AddScoped<ITestersService, TestersService>();

builder.Services.AddScoped<IDevStreamsRepository, DevStreamsRepository>();
builder.Services.AddScoped<ITestersRepository, TestersRepository>();

var app = builder.Build();

if (builder.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

app.UseHttpLogging();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();


app.Run();


// makes program class available for testing 
public partial class Program
{
}