using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json;

namespace TaskTrackerFunctions;

// Used by the EF Core command-line tools to create a DbContext for migrations.
public sealed class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("TASKS_SQL_CONNECTION_STRING")
            ?? ReadLocalSqlConnectionString()
            ?? "Server=localhost;Database=TaskTrackerDesignTime;Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure())
            .Options;

        return new TaskDbContext(options);
    }

    private static string? ReadLocalSqlConnectionString()
    {
        var localSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "local.settings.json");
        if (!File.Exists(localSettingsPath))
        {
            return null;
        }

        using var document = JsonDocument.Parse(File.ReadAllText(localSettingsPath));

        return document.RootElement
            .GetProperty("Values")
            .GetProperty("TASKS_SQL_CONNECTION_STRING")
            .GetString();
    }
}
