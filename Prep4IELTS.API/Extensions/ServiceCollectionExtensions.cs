using System.Reflection;
using CloudinaryDotNet;
using dotenv.net;
using EXE202_Prep4IELTS.Payloads.Requests;
using EXE202_Prep4IELTS.Payloads.Requests.PremiumPackage;
using EXE202_Prep4IELTS.Payloads.Requests.Tests;
using Mapster;
using MapsterMapper;
using Prep4IELTS.Business.Constants;
using Prep4IELTS.Business.Models;
using Prep4IELTS.Business.Services;
using Prep4IELTS.Business.Services.Interfaces;
using Prep4IELTS.Data;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Dtos;
using Prep4IELTS.Data.Entities;
using Serilog;

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
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IPartitionTagService, PartitionTagService>();
        services.AddScoped<ITestGradeService, TestGradeService>();
        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserPremiumPackageService, UserPremiumPackageService>();
        services.AddScoped<IPremiumPackageService, PremiumPackageService>();
        services.AddScoped<IPaymentTypeService, PaymentTypeService>();
        services.AddScoped<ISystemRoleService, SystemRoleService>();
        services.AddScoped<ISpeakingSampleService, SpeakingSampleService>();
        // services.AddScoped<ITransactionService, TransactionService>();

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
        typeAdapterConfig.NewConfig<CreateTestRequest, Test>();
        typeAdapterConfig.NewConfig<CreateTestSectionRequest, TestSection>();
        typeAdapterConfig.NewConfig<CreateTestSectionPartitionRequest, TestSectionPartition>();
        typeAdapterConfig.NewConfig<CreateQuestionRequest, Question>();
        typeAdapterConfig.NewConfig<CreateQuestionAnswerRequest, QuestionAnswer>();
        typeAdapterConfig.NewConfig<UpdateTestRequest, Test>();
        typeAdapterConfig.NewConfig<CloudResourceRequest, CloudResource>();
        typeAdapterConfig.NewConfig<UpdateTestGradeRequest, TestGrade>();
        typeAdapterConfig.NewConfig<CreatePremiumPackageRequest, PremiumPackage>();
        typeAdapterConfig.NewConfig<UpdatePremiumPackageRequest, PremiumPackage>();

        // Register the mapper as Singleton service for my application
        var mapperConfig = new Mapper(typeAdapterConfig);
        services.AddSingleton<IMapper>(mapperConfig);

        return services;
    }

    public static IServiceCollection ConfigureCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        Cloudinary cloudinary = new Cloudinary(configuration["Cloudinary:CloudinaryUrl"]!)
        {
            Api = { Secure = true }
        };

        services.AddSingleton(cloudinary);

        return services;
    }

    public static IServiceCollection EstablishApplicationConfiguration(this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        // Configure App settings
        services.Configure<AppSettings>(
            configuration.GetSection("AppSettings"));

        // Configure PayOS
        var payOsConfigSection = configuration.GetSection("PayOS");
        var payOsConfig = payOsConfigSection.Get<PayOSConfiguration>();
        if (env.IsDevelopment() && payOsConfig != null) // Is development environment
        {
            var payGate = "https://api-merchant.payos.vn";
            var paymentUrl = $"{payGate}/v2/payment-requests";
            var getPaymentLinkInformation = "https://api-merchant.payos.vn/v2/payment-requests/{0}";
            var cancelPaymentUrl = "https://api-merchant.payos.vn/v2/payment-requests/{0}/cancel";
            var confirmWebHookUrl = "https://api-merchant.payos.vn/confirm-webhook";
            var returnUrl = "http://localhost:3000/payment-return";
            var cancelUrl = "http://localhost:3000/payment-cancel";

            // var webHookUrl = "https://localhost:6000/api/payment/pay-os/webhook-return";

            services.Configure<PayOSConfiguration>(options =>
            {
                options.ClientId = payOsConfig.ClientId;
                options.ApiKey = payOsConfig.ApiKey;
                options.ChecksumKey = payOsConfig.ChecksumKey;
                options.ReturnUrl = returnUrl;
                options.CancelUrl = cancelUrl;
                options.PaymentUrl = paymentUrl;
                options.GetPaymentLinkInformationUrl = getPaymentLinkInformation;
                options.CancelPaymentUrl = cancelPaymentUrl;
                // options.WebHookUrl = webHookUrl;
                options.ConfirmWebHookUrl = confirmWebHookUrl;
            });
        }


        // Configure Momo
        var momoConfigSection = configuration.GetSection("Momo");
        var momoConfig = momoConfigSection.Get<MomoConfiguration>();
        if (env.IsDevelopment() && momoConfig != null) // Is development environment
        {
            // var returnUrl = "http://localhost:7000/api/payment/momo-return";
            var payGate = "https://test-payment.momo.vn";
            var ipnUrl = "https://localhost:6000/api/payment/momo-ipn";
            var paymentUrl = $"{payGate}/v2/gateway/api/create";
            var checkTransactionStatusUrl = $"{payGate}/v2/gateway/api/query";
            var paymentConfirmUrl = $"{payGate}/v2/gateway/api/confirm";
            var initiateTransactionUrl = $"{payGate}/v2/gateway/api/create";

            services.Configure<MomoConfiguration>(options =>
            {
                options.AccessKey = momoConfig.AccessKey;
                options.SecretKey = momoConfig.SecretKey;
                options.PaymentUrl = paymentUrl;
                options.CheckTransactionUrl = checkTransactionStatusUrl;
                options.PaymentConfirmUrl = paymentConfirmUrl;
                options.InitiateTransactionUrl = initiateTransactionUrl;
                // options.ReturnUrl = returnUrl;
                options.IpnUrl = ipnUrl;
                options.PartnerCode = momoConfig.PartnerCode;
                options.PaymentMethodName = PaymentIssuerConstants.Momo;
            });
        }

        return services;
    }
}