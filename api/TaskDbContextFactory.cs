using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskTrackerFunctions;

// Used by the EF Core command-line tools to create a DbContext for migrations.
public sealed class TaskDbContextFactory : IDesignTimeDbContextFactory<TaskDbContext>
{
    public TaskDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("TASKS_SQL_CONNECTION_STRING")
            ?? "Server=localhost;Database=TaskTrackerDesignTime;Trusted_Connection=True;TrustServerCertificate=True";

        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new TaskDbContext(options);
    }
}
