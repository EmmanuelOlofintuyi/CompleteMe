namespace CompleteMe.Application.DTO.Tasks;

public class CreateTaskRequest
{
    public Guid GoalId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ParentTaskId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? RecurrenceRuleId { get; set; }
}