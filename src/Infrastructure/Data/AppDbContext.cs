using CompleteMe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CompleteMe.Infrastructure.Data;

// DbContext is EF Core's bridge between C# entities and the database.
public class AppDbContext : DbContext
{
    // EF Core supplies these options through dependency injection.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Each DbSet represents a queryable collection/table of that entity type.
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<RecurrenceRule> RecurrenceRules => Set<RecurrenceRule>();

    // This method is where EF Core builds the database model.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Automatically finds and applies IEntityTypeConfiguration classes
        // in this Infrastructure assembly.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}