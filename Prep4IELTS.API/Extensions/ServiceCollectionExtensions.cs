using Prep4IELTS.Data;
using Prep4IELTS.Data.Context;

namespace EXE202_Prep4IELTS.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Configure/Add services 
        services.AddScoped<UnitOfWork>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        return services;
    }

    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSqlServer<Prep4IeltsContext>(
            configuration.GetConnectionString("DefaultConnectionString"));
    }
}