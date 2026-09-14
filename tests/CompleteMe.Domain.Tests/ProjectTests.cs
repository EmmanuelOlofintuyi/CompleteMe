using CompleteMe.Domain.Entities;
using Xunit;

namespace CompleteMe.Domain.Tests;

public class ProjectTests
{
    [Fact]
    public void Project_ShouldStoreCategoryAndStatusMetadata()
    {
        var projectId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var project = new Project
        {
            Id = projectId,
            Name = "Build solar bike",
            CategoryId = categoryId,
            Status = CompleteMe.Domain.Enums.ProjectStatus.Active
        };

        Assert.Equal("Build solar bike", project.Name);
        Assert.Equal(categoryId, project.CategoryId);
        Assert.Equal(CompleteMe.Domain.Enums.ProjectStatus.Active, project.Status);
    }
}
