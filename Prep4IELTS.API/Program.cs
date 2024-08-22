using System.Net.Mime;
using EXE202_Prep4IELTS.Extensions;
using EXE202_Prep4IELTS.Models;
using Microsoft.AspNetCore.Mvc;
using Prep4IELTS.Data.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure App settings
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));

// Configure DBContext
builder.Services.ConfigureDbContext(builder.Configuration)
    // Configure application services
    .AddApplicationServices();
    
// Configure CORS
builder.Services.AddCors(p => p.AddPolicy("Cors", policy => {
    // allow all with any header, method
    policy.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Debug()
    .WriteTo.Console()
    .Enrich.WithProperty("Environment", builder.Environment)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers()
    // Configure API behaviour with InvalidModelStateResponseFactory to use ModelState bad request
    // for application validation
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
            new BadRequestObjectResult(context.ModelState)
            {
                ContentTypes =
                {
                    MediaTypeNames.Application.Json,
                    MediaTypeNames.Application.Xml,
                }
            };
    })
    .AddXmlSerializerFormatters();

builder.Services.AddAuthentication();

var app = builder.Build();

// Register database initializer
app.Lifetime.ApplicationStarted.Register(() => Task.Run(async () => await app.InitializeDatabaseAsync()));
// Configure exception handler
app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("Cors");
app.UseAuthorization();
app.MapControllers();
app.Run();
