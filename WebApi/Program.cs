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
using Microsoft.IdentityModel.Tokens;
using Serilog;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StorageDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .EnableDetailedErrors());

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.
    AddApplication();


try
{

    var kvUri = "https://designs.vault.azure.net/";
    var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());


    var ppId = await client.GetSecretAsync("ppclid");
    var pps = await client.GetSecretAsync("ppcls");

    string? paypalClientIdFromVault = builder.Configuration["ppclid"];

    string? paypalClientNameFromVault = builder.Configuration["ppcls"];

    if (!string.IsNullOrEmpty(ppId.Value.Value) && !string.IsNullOrEmpty(pps.Value.Value))
    {
        builder.Configuration["AppSettings:PAYPAL_CLIENT_ID"] = paypalClientIdFromVault;
        builder.Configuration["AppSettings:PAYPAL_CLIENT_SECRET"] = paypalClientNameFromVault;
    }
}
catch(Exception ex)
{
    Log.Error(ex, "Failed to load secrets from Azure Key Vault.");
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

var corsPolicy = builder.Configuration["AppSettings:CorsPolicy"]!;
var allowedOriginsRaw = builder.Configuration["AppSettings:AllowedOrigins"];
var allowedOigins = allowedOriginsRaw?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("printfull", c =>
{
    c.BaseAddress = new Uri("https://api.printful.com/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
    c.DefaultRequestHeaders.Add("X-PF-Store-Id", builder.Configuration.GetSection("AppSettings").GetSection("PrintfullStoreID").Value);
    c.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", builder.Configuration.GetSection("AppSettings").GetSection("PrintfullToken").Value);

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

try
{
    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}


