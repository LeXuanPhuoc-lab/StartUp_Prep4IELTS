using Prep4IELTS.Data;

namespace EXE202_Prep4IELTS.Extensions;

public static class DatabaseInitializerExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();

            await initializer.InitializeAsync();
            await initializer.TrySeedAsync();
        }
    }
}