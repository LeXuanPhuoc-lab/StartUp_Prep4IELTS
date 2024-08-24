using System.Reflection;
using Mapster;
using MapsterMapper;
using Prep4IELTS.Business.Services;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Context;

namespace EXE202_Prep4IELTS.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Configure/Add services 
        services.AddScoped<UnitOfWork>();
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<ITestSectionService, TestSectionService>();
        services.AddScoped<ITestHistoryService, TestHistoryService>();
        services.AddScoped<ITestCategoryService, TestCategoryService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        return services;
    }

    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddSqlServer<Prep4IeltsContext>(
            configuration.GetConnectionString("DefaultConnectionString"));
    }

    public static IServiceCollection ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Default
            .MapToConstructor(true)
            .PreserveReference(true);
        // Get Mapster GlobalSettings
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        // Scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
        typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
        // Register the mapper as Singleton service for my application
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);

        return services;
    }
}