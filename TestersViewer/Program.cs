using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using TestersViewer.Filters.ActionFilters;
using TestersViewer.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.ConfigureServices(builder.Configuration);



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