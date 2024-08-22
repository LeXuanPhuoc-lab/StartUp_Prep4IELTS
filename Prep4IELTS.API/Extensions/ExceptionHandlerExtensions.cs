using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace EXE202_Prep4IELTS.Extensions;

public static class ExceptionHandlerExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandlerFeature != null)
                {
                    // (string title, string? detail) errorDetails = new ("", "");
                    //
                    // if (app.Environment.IsDevelopment())
                    // {
                    //     errorDetails.title = exceptionHandlerFeature.Error.Message;
                    //     errorDetails.detail = exceptionHandlerFeature.Error.StackTrace;
                    // }
                    // else
                    // {
                    //     errorDetails.title = "An error occured while processing your request.";
                    // }
                    
                    var errorDetails = new
                    {
                        title = app.Environment.IsDevelopment()
                            ? exceptionHandlerFeature.Error.Message
                            : "An error occurred while processing your request.",
                        detail = app.Environment.IsDevelopment()
                            ? exceptionHandlerFeature.Error.StackTrace
                            : null
                    };
                    await context.Response.WriteAsJsonAsync(errorDetails);
                }
            });
        });
    }
}