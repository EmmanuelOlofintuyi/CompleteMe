using CompleteMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompleteMe.Infrastructure.Configuration;

// Maps TaskItem properties and its goal, parent-task, category, and recurrence relationships.
public class TaskItemConfig : IEntityTypeConfiguration<TaskItem>
{
    // EF Core calls this while constructing the model.
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        // Store tasks in the TaskItems table.
        builder.ToTable("TaskItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc)
            .IsRequired();

        // GoalId is required, so every task belongs to a goal.
        // Deleting a goal also deletes its tasks.
        builder.HasOne<Goal>()
            .WithMany(x => x.Tasks)
            .HasForeignKey(x => x.GoalId)
            .OnDelete(DeleteBehavior.Cascade);

        // ParentTaskId is optional, allowing both top-level and nested tasks.
        // Restrict avoids the cascade cycle between Goal -> TaskItems and TaskItems -> ParentTask.
        builder.HasOne(x => x.ParentTask)
            .WithMany(x => x.ChildTasks)
            .HasForeignKey(x => x.ParentTaskId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category and recurrence rule are optional task metadata.
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne<RecurrenceRule>()
            .WithMany()
            .HasForeignKey(x => x.RecurrenceRuleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}