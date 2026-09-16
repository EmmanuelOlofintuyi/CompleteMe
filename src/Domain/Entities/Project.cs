using CompleteMe.Domain.Enums;
namespace CompleteMe.Domain.Entities;

// A Project is an optional grouping container for related goals.
public class Project
{
    // Primary key used to identify this project.
    public Guid Id {get; set;}

    // Required display name for the project.
    public string Name {get; set;} = string.Empty;

    // Optional supporting information about the project.
    public string ? Description {get; set;}
    public DateTime? StartDate{get; set;}
    public DateTime? DueDate{get; set;}

    // Stored in UTC so timestamps are consistent regardless of the user's location.
    public DateTime CreatedAtUtc {get; set;}
    public DateTime UpdatedAtUtc {get; set;}

    // A project may optionally be assigned to a category.
    public Guid? CategoryId {get; set;}

    // The enum represents the project's lifecycle.
    public ProjectStatus Status {get; set;}

    // Navigation collection: a project can contain multiple goals.
    public ICollection<Goal> Goals { get; set; } = new List<Goal>();
}