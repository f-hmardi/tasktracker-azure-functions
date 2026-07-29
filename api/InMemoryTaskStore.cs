using System.Collections.Concurrent;

namespace TaskTrackerFunctions;

public sealed class InMemoryTaskStore
{
    private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

    public IReadOnlyCollection<TaskItem> GetAll() => _tasks.Values
        .OrderByDescending(task => task.CreatedAtUtc)
        .ToArray();

    public TaskItem Create(string title, string? description)
    {
        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim()
        };

        _tasks[task.Id] = task;
        return task;
    }
}
