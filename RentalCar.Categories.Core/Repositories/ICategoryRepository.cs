using RentalCar.Categories.Core.Entities;

namespace RentalCar.Categories.Core.Repositories;

public interface ICategoryRepository
{
    Task<Category> Create(Category category, CancellationToken cancellationToken);
    Task Update(Category category, CancellationToken cancellationToken);
    Task Delete(Category category, CancellationToken cancellationToken);
    Task<bool> IsCategoryExist(string name, CancellationToken cancellationToken);
    Task<Category?> GetById(string id, CancellationToken cancellationToken);
    Task<List<Category>> GetAll(CancellationToken cancellationToken);
}