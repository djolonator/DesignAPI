using Application;
using Application.Repositories;
using Application.Services;
using Application.Services.External;
using Application.Validations;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Domain;
using FluentValidation;
using Infrastracture.Interfaces.IRepositories;
using Infrastracture.Interfaces.IServices;
using Infrastracture.Interfaces.IServices.External;
using Infrastracture.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StorageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableDetailedErrors());

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.
    AddApplication();


var keyVaultUri = new Uri("https://designs.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

string? paypalClientIdFromVault = builder.Configuration["ppclid"];

string? paypalClientNameFromVault = builder.Configuration["ppcls"];

if (!string.IsNullOrEmpty(paypalClientIdFromVault) && !string.IsNullOrEmpty(paypalClientNameFromVault))
{
    builder.Configuration["AppSettings:PAYPAL_CLIENT_ID"] = paypalClientIdFromVault;
    builder.Configuration["AppSettings:PAYPAL_CLIENT_SECRET"] = paypalClientNameFromVault;
}


builder.Services.AddScoped<IDesignRepository, DesignRepository>();
builder.Services.AddScoped<IPosterService, PosterService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

//external
builder.Services.AddScoped<IPrintfullService, PrintfullService>();
builder.Services.AddScoped<IPayPallService, PayPallService>();


builder.Services.AddScoped<IValidator<CheckoutRequest>, CheckoutValidator>();
builder.Services.AddScoped<IValidator<CartItemModel>, CartItemValidator>();
builder.Services.AddScoped<IValidator<RecipientModel>, RecipientValidator>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllers();

var corsPolicy = builder.Configuration["AppSettings:CORS_POLICY"]!;
var allowedOigins = builder.Configuration.GetSection("AppSettings:ALLOWED_ORIGINS").Get<string[]>();
builder.Services.AddCors(options =>
{
    
    options.AddPolicy(corsPolicy,
        builder =>
        {
            builder.WithOrigins(allowedOigins!)
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

app.UseSerilogRequestLogging();
app.MapIdentityApi<IdentityUser>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(corsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
