using Xunit;

namespace TaskTrackerFunctions.Tests;

public sealed class InMemoryTaskStoreTests
{
    [Fact]
    public async Task CreateAsync_TrimsValuesAndReturnsCreatedTask()
    {
        var store = new InMemoryTaskStore();

        var task = await store.CreateAsync("  Learn testing  ", "  Write a unit test  ");

        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal("Learn testing", task.Title);
        Assert.Equal("Write a unit test", task.Description);
        Assert.Equal("Todo", task.Status);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTasksCreatedInTheStore()
    {
        var store = new InMemoryTaskStore();
        var first = await store.CreateAsync("First task", null);
        var second = await store.CreateAsync("Second task", "A description");

        var tasks = await store.GetAllAsync();

        Assert.Equal(2, tasks.Count);
        Assert.Contains(tasks, task => task.Id == first.Id);
        Assert.Contains(tasks, task => task.Id == second.Id);
    }
}
