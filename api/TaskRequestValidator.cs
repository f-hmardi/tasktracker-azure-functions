namespace TaskTrackerFunctions;

public static class TaskRequestValidator
{
    public const string InvalidTitleMessage = "Title is required and must contain at least 3 characters.";

    public static string? Validate(CreateTaskRequest? request)
    {
        return string.IsNullOrWhiteSpace(request?.Title) || request.Title.Trim().Length < 3
            ? InvalidTitleMessage
            : null;
    }
}
