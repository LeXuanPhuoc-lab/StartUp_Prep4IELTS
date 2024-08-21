using EXE202_Prep4IELTS.Extensions;
using Prep4IELTS.Data.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddAuthentication();

var app = builder.Build();

// Register database initializer
app.Lifetime.ApplicationStarted.Register(() => Task.Run(async () => await app.InitializeDatabaseAsync()));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("Cors");
app.UseAuthorization();
app.MapControllers();
app.Run();
