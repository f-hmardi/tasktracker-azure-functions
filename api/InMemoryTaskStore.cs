using System.Collections.Concurrent;

namespace TaskTrackerFunctions;

public interface ITaskStore
{
    Task<IReadOnlyCollection<TaskItem>> GetAllAsync();
    Task<TaskItem> CreateAsync(string title, string? description);
}

public sealed class InMemoryTaskStore : ITaskStore
{
    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

    public Task<IReadOnlyCollection<TaskItem>> GetAllAsync() => Task.FromResult<IReadOnlyCollection<TaskItem>>(_tasks.Values
        .OrderByDescending(task => task.CreatedAtUtc)
        .ToArray());

    public Task<TaskItem> CreateAsync(string title, string? description)
    {
        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim()
        };

        _tasks[task.Id] = task;
        return Task.FromResult(task);
    }
}
