using Application;
using Application.Repositories;
using Application.Services;
using Domain;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ParkingDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ParkingDB;Trusted_Connection=True;"));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

//builder.Services.
//    AddInfrastructure().
//    AddApplication();

//Za sada neka ga ovde
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3001",
        builder =>
        {
            builder.WithOrigins("http://localhost:3001")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAutoMapper(typeof(ParkingMapper));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
