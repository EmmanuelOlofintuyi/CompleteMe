namespace CompleteMe.Domain.Entities;
public class RecurrenceRule
{
    public Guid Id {get; set;}
    public Guid TaskItemId {get; set;}
    public string Rule {get; set;} = string.Empty;
}