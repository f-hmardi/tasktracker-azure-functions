namespace TaskTrackerFunctions;

public sealed class TaskItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string Status { get; init; } = "Todo";
    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;
}

public sealed class CreateTaskRequest
{
    public string? Title { get; init; }
    public string? Description { get; init; }
}
