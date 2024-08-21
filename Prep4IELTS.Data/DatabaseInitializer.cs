using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Context;

namespace Prep4IELTS.Data;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
    Task TrySeedAsync();
    Task SeedAsync();
}

public class DatabaseInitializer(Prep4IeltsContext dbContext) : IDatabaseInitializer
{
    public async Task InitializeAsync()
    {
        try
        {
            // Check whether database exist
            if (!await dbContext.Database.CanConnectAsync())
            {
                // Perform migration database
                await dbContext.Database.MigrateAsync();
            }
            
            // Check for applied migrations
            var appliedMigrations = await dbContext.Database.GetAppliedMigrationsAsync();
            if (appliedMigrations.Any())
            {
                Console.WriteLine("Migrations have been applied.");
                return;
            }
            Console.WriteLine("Database initialized successfully");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task TrySeedAsync()
    {
        try
        {
            await SeedAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}