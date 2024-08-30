using System.Reflection;
using EXE202_Prep4IELTS.Payloads.Requests;
using Mapster;
using MapsterMapper;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Extensions;

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
        services.AddScoped<ITestPartitionHistoryService, TestPartitionHistoryService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IScoreCalculationService, ScoreCalculationService>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        
        // Register IHttpContextAccessor 
        services.AddHttpContextAccessor();
        
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
        
        // Additional mapping custom from API to Business layer
        typeAdapterConfig.NewConfig<QuestionAnswerSubmissionRequest, QuestionAnswerSubmissionModel>();
        
        // Register the mapper as Singleton service for my application
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);

        return services;
    }

    public static IServiceCollection EstablishApplicationConfiguration(this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        // Configure App settings
        services.Configure<AppSettings>(
            configuration.GetSection("AppSettings"));
        
        // Configure Momo
        var momoConfigSection = configuration.GetSection("Momo");
        var momoConfig = momoConfigSection.Get<MomoConfiguration>();
        if (env.IsDevelopment() && momoConfig != null) // Is development environment
        {
            var returnUrl = "http://localhost:7000/api/payment/momo-return";
            var ipnUrl = "http://localhost:7000/api/payment/momo-ipn";
            var paymentUrl = "https://test-payment.momo.vn/v2/gateway/api/create";

            services.Configure<MomoConfiguration>(options =>
            {
                options.AccessKey = momoConfig.AccessKey;
                options.SecretKey = momoConfig.SecretKey;
                options.PaymentUrl = paymentUrl;
                options.ReturnUrl = returnUrl;
                options.IpnUrl = ipnUrl;
                options.PartnerCode = momoConfig.PartnerCode;
                options.PaymentMethodName = PaymentMethodConstants.Momo;
            });
        }
        
        return services;
    }
}