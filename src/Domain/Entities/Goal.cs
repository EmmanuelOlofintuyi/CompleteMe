using CompleteMe.Domain.Enums;
namespace CompleteMe.Domain.Entities;

// A Goal is the main planning object in CompleteMe.
// A goal can exist by itself or be grouped inside an optional Project.
public class Goal
{
    // Every entity needs a stable identifier so it can be stored and found later.
    public Guid Id {get; set;}

    // The required, human-readable name of the goal.
    public string Name {get; set;} = string.Empty;

    // The description is optional, so the value can be null.
    public string ? Description {get; set;}

    // Dates are optional because a user may create a goal before planning its schedule.
    public DateTime? StartDate{get; set;}
    public DateTime? DueDate{get; set;}

    // This nullable foreign key makes the Project relationship optional.
    public Guid? ProjectId {get; set;}

    // The enum keeps the goal lifecycle limited to known states.
    public GoalStatus Status {get; set;}

    // UTC timestamps make stored times consistent across machines and time zones.
    public DateTime CreatedAtUtc {get; set;}
    public DateTime UpdatedAtUtc {get; set;}

    // These optional foreign keys connect a goal to supporting data.
    public Guid? CategoryId {get; set;}
    public Guid? RecurrenceRuleId {get; set;}

    // Navigation property: this is the related Project object in C#.
    // ProjectId is the value stored as the database foreign key.
    public Project ? Project {get; set;}

    // Navigation collection: one goal can contain many tasks.
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}