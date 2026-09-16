using CompleteMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompleteMe.Infrastructure.Configuration;

// Maps Project properties and relationships to the database.
public class ProjectConfig : IEntityTypeConfiguration<Project>
{
    // EF Core calls this while constructing the model.
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // Store projects in the Projects table.
        builder.ToTable("Projects");

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

        // A project can contain many goals.
        // If the project is deleted, its goals remain as standalone goals.
        builder.HasMany(x => x.Goals)
            .WithOne(x => x.Project)
            .OnDelete(DeleteBehavior.SetNull);
    }
}