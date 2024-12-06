using Application;
using Application.Repositories;
using Application.Services;
using Application.Validations;
using Domain;
using FluentValidation;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StorageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.
    AddApplication();

//Za sada neka ga ovde

builder.Services.AddScoped<IDesignRepository, DesignRepository>();
builder.Services.AddScoped<IDesignService, DesignService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();


builder.Services.AddScoped<IValidator<CheckoutRequest>, CheckoutValidator>();
builder.Services.AddScoped<IValidator<CartItemModel>, CartItemValidator>();
builder.Services.AddScoped<IValidator<RecipientModel>, RecipientValidator>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddAutoMapper(typeof(PostersMapper));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("printfull", c =>
{
    c.BaseAddress = new Uri("https://api.printful.com/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.DefaultRequestHeaders.Add("X-PF-Store-Id", builder.Configuration.GetSection("AppSettings").GetSection("STORE_ID").Value);
    c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", builder.Configuration.GetSection("AppSettings").GetSection("PRINTFULL_TOKEN").Value);

});
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<StorageDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseCors("AllowLocalhost3000");
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
