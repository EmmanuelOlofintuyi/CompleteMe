namespace CompleteMe.Domain.Enums;

// Known lifecycle states for a concrete task.
public enum TaskItemStatus
{
    NotStarted,
    InProgress,
    Completed,
    Dismissed,
    Rescheduled,
    Overdue
}