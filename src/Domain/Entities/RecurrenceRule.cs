namespace CompleteMe.Domain.Entities;

// Stores the rule that describes how a recurring item should repeat.
public class RecurrenceRule
{
    // Primary key for the recurrence rule.
    public Guid Id {get; set;}

    // Identifies the task associated with this rule.
    public Guid TaskItemId {get; set;}

    // The rule text is kept separate from the task so recurrence behavior can evolve independently.
    public string Rule {get; set;} = string.Empty;
}