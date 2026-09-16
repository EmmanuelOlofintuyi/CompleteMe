using CompleteMe.Domain.Enums;

namespace CompleteMe.Application.DTOs.Goals;

public class GoalResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid? ProjectId { get; set; }
    public GoalStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? RecurrenceRuleId { get; set; }
}