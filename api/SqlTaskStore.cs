using Microsoft.EntityFrameworkCore;

namespace TaskTrackerFunctions;

public sealed class SqlTaskStore(IDbContextFactory<TaskDbContext> dbContextFactory) : ITaskStore
{
    public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync()
    {
        await using var db = await CreateDatabaseAsync();

        return await db.Tasks
            .AsNoTracking()
            .OrderByDescending(task => task.CreatedAtUtc)
            .ToArrayAsync();
    }

    public async Task<TaskItem> CreateAsync(string title, string? description)
    {
        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim()
        };

        await using var db = await CreateDatabaseAsync();
        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        return task;
    }

    private async Task<TaskDbContext> CreateDatabaseAsync()
    {
        var db = await dbContextFactory.CreateDbContextAsync();
        await db.Database.EnsureCreatedAsync();
        return db;
    }
}
