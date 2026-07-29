using Azure.Data.Tables;

namespace TaskTrackerFunctions;

public sealed class AzureTableTaskStore : ITaskStore
{
    private const string PartitionKey = "tasks";
    private readonly TableClient _table;

    public AzureTableTaskStore(string connectionString)
    {
        _table = new TableClient(connectionString, "Tasks");
    }

    public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync()
    {
        await _table.CreateIfNotExistsAsync();
        var tasks = new List<TaskItem>();

        await foreach (var entity in _table.QueryAsync<TableEntity>(filter: $"PartitionKey eq '{PartitionKey}'"))
        {
            tasks.Add(new TaskItem
            {
                Id = Guid.Parse(entity.RowKey),
                Title = entity.GetString("Title") ?? "Untitled task",
                Description = entity.GetString("Description"),
                Status = entity.GetString("Status") ?? "Todo",
                CreatedAtUtc = entity.GetDateTimeOffset("CreatedAtUtc") ?? DateTimeOffset.UtcNow
            });
        }

        return tasks.OrderByDescending(task => task.CreatedAtUtc).ToArray();
    }

    public async Task<TaskItem> CreateAsync(string title, string? description)
    {
        await _table.CreateIfNotExistsAsync();

        var task = new TaskItem
        {
            Title = title.Trim(),
            Description = description?.Trim()
        };

        var entity = new TableEntity(PartitionKey, task.Id.ToString())
        {
            ["Title"] = task.Title,
            ["Description"] = task.Description,
            ["Status"] = task.Status,
            ["CreatedAtUtc"] = task.CreatedAtUtc
        };

        await _table.AddEntityAsync(entity);
        return task;
    }
}
