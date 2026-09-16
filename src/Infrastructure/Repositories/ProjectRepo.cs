using CompleteMe.Domain.Entities;
using CompleteMe.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompleteMe.Infrastructure.Repositories;

// Defines the project data operations used by the application layer.
public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync();
    Task<Project?> GetByIdAsync(Guid id);
    Task<List<Project>> GetByCategoryIdAsync(Guid categoryId);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(Guid id);
}

// Implements project data access using EF Core.
public class ProjectRepo : IProjectRepository
{
    // The context provides access to the Projects table.
    private readonly AppDbContext _context;

    public ProjectRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync()
    {
        // Include loads the goals belonging to each project.
        return await _context.Projects
            .Include(p => p.Goals)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        // Return the matching project, or null when it does not exist.
        return await _context.Projects
            .Include(p => p.Goals)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Project>> GetByCategoryIdAsync(Guid categoryId)
    {
        // Filter projects by their optional category foreign key.
        return await _context.Projects
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync();
    }

    public async Task AddAsync(Project project)
    {
        // Mark the project for insertion, then commit the change.
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project)
    {
        // Mark the project's current values for update, then commit them.
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        // Find the tracked entity before asking EF Core to delete it.
        var project = await _context.Projects.FindAsync(id);
        if (project is null)
            return;

        _context.Projects.Remove(project);
        // SaveChangesAsync sends the deletion to the database.
        await _context.SaveChangesAsync();
    }
}