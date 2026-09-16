using CompleteMe.Domain.Enums;
namespace CompleteMe.Domain.Entities;

// A TaskItem represents a concrete piece of work that belongs to a Goal.
public class TaskItem
{
    // Primary key for this task.
    public Guid Id {get; set;}

    // Required foreign key: every task must belong to a goal.
    public Guid GoalId {get; set;}

    // Required display name for the task.
    public string Name {get; set;} = string.Empty;

    // Optional details explaining what needs to be done.
    public string ? Description {get; set;}
    public DateTime? StartDate{get; set;}
    public DateTime? DueDate{get; set;}

    // The enum prevents arbitrary text from representing task status.
    public TaskItemStatus Status {get; set;}

    // Stored in UTC for consistent persistence and comparison.
    public DateTime CreatedAtUtc {get; set;}
    public DateTime UpdatedAtUtc {get; set;}

    // Optional recurrence information for repeating work.
    public RecurrenceFrequency? RecurrenceFrequency {get; set;}
    public Guid? CategoryId {get; set;}
    public Guid? RecurrenceRuleId {get; set;}

    // Nullable because a top-level task does not have a parent task.
    public Guid? ParentTaskId {get; set;}

    // Navigation property for the task's optional parent.
    public TaskItem ? ParentTask {get; set;}

    // Navigation collection for all tasks directly nested under this task.
    public ICollection<TaskItem> ChildTasks { get; set; } = new List<TaskItem>();
}