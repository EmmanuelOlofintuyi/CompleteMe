using CompleteMe.Domain.Entities;
using CompleteMe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompleteMe.Infrastructure.Repositories;

// Defines the goal data operations that the application layer may use.
public interface IGoalRepository
{
    Task<List<Goal>> GetAllAsync();
    Task<Goal?> GetByIdAsync(Guid id);
    Task<List<Goal>> GetByProjectIdAsync(Guid projectId);
    Task AddAsync(Goal goal);
    Task UpdateAsync(Goal goal);
    Task DeleteAsync(Guid id);
}

// Implements goal data access using EF Core.
public class GoalRepo : IGoalRepository
{
    // The context tracks entities and sends queries/changes to the database.
    private readonly AppDbContext _context;

    public GoalRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Goal>> GetAllAsync()
    {
        // Include loads the related project in the same query.
        return await _context.Goals
            .Include(g => g.Project)
            .ToListAsync();
    }

    public async Task<Goal?> GetByIdAsync(Guid id)
    {
        // FirstOrDefaultAsync returns null when no goal matches the id.
        return await _context.Goals
            .Include(g => g.Project)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<List<Goal>> GetByProjectIdAsync(Guid projectId)
    {
        // Filter by the foreign-key value to find goals in one project.
        return await _context.Goals
            .Where(g => g.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task AddAsync(Goal goal)
    {
        // Add marks the entity for insertion; SaveChangesAsync commits it.
        _context.Goals.Add(goal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Goal goal)
    {
        // Update tells EF Core to persist the current values of this entity.
        _context.Goals.Update(goal);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        // Find the entity first so EF Core knows which tracked row to remove.
        var goal = await _context.Goals.FindAsync(id);
        if (goal is null)
            return;

        _context.Goals.Remove(goal);
        // SaveChangesAsync sends the deletion to the database.
        await _context.SaveChangesAsync();
 
    }
}