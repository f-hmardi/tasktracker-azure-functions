using Xunit;

namespace TaskTrackerFunctions.Tests;

public sealed class TaskRequestValidatorTests
{
    [Fact]
    public void Validate_WhenRequestIsMissing_ReturnsValidationError()
    {
        var error = TaskRequestValidator.Validate(null);

        Assert.Equal(TaskRequestValidator.InvalidTitleMessage, error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("ab")]
    public void Validate_WhenTitleIsInvalid_ReturnsValidationError(string? title)
    {
        var error = TaskRequestValidator.Validate(new CreateTaskRequest { Title = title });

        Assert.Equal(TaskRequestValidator.InvalidTitleMessage, error);
    }

    [Fact]
    public void Validate_WhenTitleHasAtLeastThreeCharacters_ReturnsNoError()
    {
        var error = TaskRequestValidator.Validate(new CreateTaskRequest { Title = "Learn Docker" });

        Assert.Null(error);
    }
}
