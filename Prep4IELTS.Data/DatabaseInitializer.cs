using Microsoft.EntityFrameworkCore;
using Prep4IELTS.Data.Context;
using Prep4IELTS.Data.Entities;
using Prep4IELTS.Data.Extensions;

namespace Prep4IELTS.Data;

public interface IDatabaseInitializer
{
    Task InitializeAsync();
    Task TrySeedAsync();
    Task SeedAsync();
}

public class DatabaseInitializer(Prep4IeltsContext dbContext) : IDatabaseInitializer
{
    //  Summary:
    //      Initialize Database
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

    //  Summary:
    //      Try to perform seeding data
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
    
    //  Summary:
    //      Seeding data
    public async Task SeedAsync()
    {
        try
        {
            // System roles
            if (!dbContext.SystemRoles.Any()) await SeedSystemRoleAsync();
            // Tags
            if (!dbContext.Tags.Any()) await SeedTagAsync();
            // Test categories
            if (!dbContext.TestCategories.Any()) await SeedTestCategoryAsync();
                
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    
    //  Summary:
    //      Seeding system roles
    private async Task SeedSystemRoleAsync()
    {
        List<SystemRole> roles = new()
        {
            new() { RoleName = Enum.SystemRole.Admin.GetDescription() },
            new() { RoleName = Enum.SystemRole.Staff.GetDescription() },
            new() { RoleName = Enum.SystemRole.Student.GetDescription() }
        };

        await dbContext.SystemRoles.AddRangeAsync(roles);
        await dbContext.SaveChangesAsync();
    }
    
    //  Summary:
    //      Seeding tags 
    private async Task SeedTagAsync()
    {
        List<Tag> tags = new()
        {
            new() { TagName = Enum.Tag.IeltsAcademic.GetDescription() },
            new() { TagName = Enum.Tag.IeltsGeneral.GetDescription() },
            new() { TagName = Enum.Tag.Reading.GetDescription() },
            new() { TagName = Enum.Tag.Listening.GetDescription() },
            new() { TagName = Enum.Tag.Writing.GetDescription() },
            new() { TagName = Enum.Tag.Speaking.GetDescription() }
        };
        
        await dbContext.Tags.AddRangeAsync(tags);
        await dbContext.SaveChangesAsync();
    }
        
    //  Summary:
    //      Seeding Test Categories
    private async Task SeedTestCategoryAsync()
    {
        List<TestCategory> categories = new()
        {
            new() { TestCategoryName = Enum.TestCategory.IeltsAcademic.GetDescription() },
            new() { TestCategoryName = Enum.TestCategory.IeltsGeneral.GetDescription() }
        };
        
        await dbContext.TestCategories.AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }
    
    
}