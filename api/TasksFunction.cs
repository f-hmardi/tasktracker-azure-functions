using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskTrackerFunctions;

public class TasksFunction
{
    private readonly ILogger<TasksFunction> _logger;
    private readonly ITaskStore _taskStore;

    public TasksFunction(ILogger<TasksFunction> logger, ITaskStore taskStore)
    {
        _logger = logger;
        _taskStore = taskStore;
    }

    [Function("GetTasks")]
    public async Task<IActionResult> GetTasks(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "tasks")] HttpRequest req)
    {
        var tasks = await _taskStore.GetAllAsync();
        _logger.LogInformation("Returned {TaskCount} tasks.", tasks.Count);

        return new OkObjectResult(tasks);
    }

    [Function("CreateTask")]
    public async Task<IActionResult> CreateTask(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "tasks")] HttpRequest req)
    {
        var request = await req.ReadFromJsonAsync<CreateTaskRequest>();
        var validationError = TaskRequestValidator.Validate(request);

        if (validationError is not null)
        {
            _logger.LogWarning("Task creation was rejected because the title was missing or too short.");

            return new BadRequestObjectResult(new
            {
                error = validationError
            });
        }

        var task = await _taskStore.CreateAsync(request!.Title!, request.Description);
        _logger.LogInformation("Created task {TaskId}.", task.Id);

        return new CreatedResult($"/api/tasks/{task.Id}", task);
    }
}
