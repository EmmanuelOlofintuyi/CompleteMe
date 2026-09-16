using CompleteMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompleteMe.Infrastructure.Configuration;

// Keeps Goal's database mapping separate from the domain entity.
public class GoalConfig : IEntityTypeConfiguration<Goal>
{
    // EF Core calls this method while building the database model.
    public void Configure(EntityTypeBuilder<Goal> builder)
    {
        // Map Goal to the Goals table.
        builder.ToTable("Goals");

        // Id becomes the table's primary key.
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

        // One goal can have many tasks, and a task must have a goal.
        // Deleting a goal also deletes its tasks.
        builder.HasMany(x => x.Tasks)
            .WithOne()
            .HasForeignKey(x => x.GoalId)
            .OnDelete(DeleteBehavior.Cascade);

        // ProjectId is nullable, so a goal may exist without a project.
        // Deleting a project removes the association but keeps the goal.
        builder.HasOne<Project>()
            .WithMany(x => x.Goals)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        // Category and recurrence are optional supporting relationships.
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