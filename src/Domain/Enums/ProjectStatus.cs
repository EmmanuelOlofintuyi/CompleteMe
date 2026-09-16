namespace CompleteMe.Domain.Enums;

// Lifecycle states that describe the current condition of a project.
public enum ProjectStatus
{
    Active,
    Paused,
    Blocked,
    Completed,
    Archived,
    Dismissed
}