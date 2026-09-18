using System.Net;
using System.Net.Http.Json;
using CompleteMe.Application.DTO.Tasks;
using CompleteMe.Application.DTOs.Goals;
using CompleteMe.Domain.Enums;

namespace CompleteMe.Api.Tests;

public class GoalsApiTests
{
    [Fact]
    public async Task CompleteGoal_WithIncompleteTask_ReturnsConflict()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var createGoalRequest = new CreateGoalRequest
        {
            Name = "Launch app",
            Description = "Build and ship the MVP",
            StartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7),
            ProjectId = null
        };

        var goalResponse = await client.PostAsJsonAsync("/api/Goals", createGoalRequest);
        var createdGoal = await goalResponse.Content.ReadFromJsonAsync<GoalResponse>();

        Assert.Equal(HttpStatusCode.Created, goalResponse.StatusCode);
        Assert.NotNull(createdGoal);

        var createTaskRequest = new
        {
            Name = "Design the dashboard",
            Description = "Create the UX for the landing page",
            GoalId = createdGoal!.Id
        };

        var taskResponse = await client.PostAsJsonAsync("/api/Task", createTaskRequest);
        Assert.Equal(HttpStatusCode.Created, taskResponse.StatusCode);

        var completeResponse = await client.PostAsync($"/api/Goals/{createdGoal.Id}/complete", null);

        Assert.Equal(HttpStatusCode.Conflict, completeResponse.StatusCode);
    }

    [Fact]
    public async Task CompleteGoal_WhenAllTasksAreComplete_ReturnsOk()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var createGoalRequest = new CreateGoalRequest
        {
            Name = "Finish sprint",
            Description = "Ship the release",
            StartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(5)
        };

        var goalResponse = await client.PostAsJsonAsync("/api/Goals", createGoalRequest);
        var createdGoal = await goalResponse.Content.ReadFromJsonAsync<GoalResponse>();

        Assert.Equal(HttpStatusCode.Created, goalResponse.StatusCode);
        Assert.NotNull(createdGoal);

        var createTaskRequest = new
        {
            GoalId = createdGoal!.Id,
            Name = "Deploy release",
            Description = "Prepare and deploy the app"
        };

        var taskResponse = await client.PostAsJsonAsync("/api/Task", createTaskRequest);
        Assert.Equal(HttpStatusCode.Created, taskResponse.StatusCode);

        var createdTask = await taskResponse.Content.ReadFromJsonAsync<TaskResponse>();
        Assert.NotNull(createdTask);

        var updateTaskRequest = new
        {
            Name = createdTask!.Name,
            Description = createdTask.Description,
            StartDate = createdTask.StartDate,
            DueDate = createdTask.DueDate,
            ParentTaskId = createdTask.ParentTaskId,
            CategoryId = createdTask.CategoryId,
            RecurrenceRuleId = createdTask.RecurrenceRuleId,
            Status = TaskItemStatus.Completed
        };

        var updateTaskResponse = await client.PutAsJsonAsync($"/api/Task/{createdTask.Id}", updateTaskRequest);
        Assert.Equal(HttpStatusCode.OK, updateTaskResponse.StatusCode);

        var completeResponse = await client.PostAsync($"/api/Goals/{createdGoal.Id}/complete", null);

        Assert.Equal(HttpStatusCode.OK, completeResponse.StatusCode);
    }

    [Fact]
    public async Task CompleteGoal_WithNoTasks_ReturnsOk()
    {
        await using var factory = new CustomWebApplicationFactory();
        using var client = factory.CreateClient();

        var createGoalRequest = new CreateGoalRequest
        {
            Name = "Ship release",
            Description = "Finalize a goal with no tasks",
            StartDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(3)
        };

        var goalResponse = await client.PostAsJsonAsync("/api/Goals", createGoalRequest);
        var createdGoal = await goalResponse.Content.ReadFromJsonAsync<GoalResponse>();

        Assert.Equal(HttpStatusCode.Created, goalResponse.StatusCode);
        Assert.NotNull(createdGoal);

        var completeResponse = await client.PostAsync($"/api/Goals/{createdGoal!.Id}/complete", null);

        Assert.Equal(HttpStatusCode.OK, completeResponse.StatusCode);
    }
}

