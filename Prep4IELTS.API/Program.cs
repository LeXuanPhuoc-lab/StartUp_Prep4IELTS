using System.Net.Mime;
using System.Text.Json;
using EXE202_Prep4IELTS.Extensions;
using EXE202_Prep4IELTS.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Configure DBContext
builder.Services.ConfigureDbContext(builder.Configuration)
    // Configure application services
    .AddApplicationServices();

// Configure App settings
builder.Services.EstablishApplicationConfiguration(
    builder.Configuration, builder.Environment);

// Configure Cloudinary
builder.Services.ConfigureCloudinary();
    
// Configure CORS
builder.Services.AddCors(p => p.AddPolicy("Cors", policy =>
{
    // allow all with any header, method
    policy.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

// Configure Mapster
builder.Services.ConfigureMapster();

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
        {
            // Convert model state keys to camel case
            var errors = context.ModelState
                .ToDictionary(
                    kvp => JsonNamingPolicy.CamelCase.ConvertName(kvp.Key), // Convert key to camel case
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() // Get the error messages
                );

            var result = new UnprocessableEntityObjectResult(errors)
            {
                ContentTypes =
                {
                    MediaTypeNames.Application.Json,
                    MediaTypeNames.Application.Xml,
                }
            };

            return result;
            // new UnprocessableEntityObjectResult(context.ModelState)
            // {
            //     ContentTypes =
            //     {
            //         MediaTypeNames.Application.Json,
            //         MediaTypeNames.Application.Xml,
            //     }
            // };
        };
    })
    .AddXmlSerializerFormatters();

builder.Services.AddClerkConfigure();

var app = builder.Build();

// Register database initializer
app.Lifetime.ApplicationStarted.Register(() => Task.Run(async () => await app.InitializeDatabaseAsync()));
// Configure exception handler
app.ConfigureExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("Cors");
app.UseMiddleware<ClerkAuthMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();