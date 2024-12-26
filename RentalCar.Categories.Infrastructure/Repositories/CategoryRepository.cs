using Microsoft.EntityFrameworkCore;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Infrastructure.Persistence;

namespace RentalCar.Categories.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryContext _context;

    public CategoryRepository(CategoryContext context)
    {
        _context = context;
    }

    public async Task<Category> Create(Category category, CancellationToken cancellationToken)
    {
        _context.Category.Add(category);
        await _context.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task Delete(Category category, CancellationToken cancellationToken)
    {
        category.IsDeleted = true;
        category.DeletedAt = DateTime.UtcNow;
        _context.Category.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Category>> GetAll(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Category
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetById(string id, CancellationToken cancellationToken)
    {
        return await _context.Category.FirstOrDefaultAsync(c => !c.IsDeleted && string.Equals(c.Id, id), cancellationToken);
    }

    public async Task<bool> IsCategoryExist(string name, CancellationToken cancellationToken)
    {
        return await _context.Category.AnyAsync(c => string.Equals(c.Name, name), cancellationToken);
    }

    public async Task Update(Category category, CancellationToken cancellationToken)
    {
        category.UpdatedAt = DateTime.UtcNow;
        _context.Category.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }
}