using RentalCar.Categories.Core.Entities;

namespace RentalCar.Categories.Core.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Create(Category Category, CancellationToken cancellationToken);
        Task Update(Category Category, CancellationToken cancellationToken);
        Task Delete(Category Category, CancellationToken cancellationToken);
        Task<bool> IsCategoryExist(string name, CancellationToken cancellationToken);
        Task<Category?> GetById(string Id, CancellationToken cancellationToken);
        Task<List<Category>> GetAll(CancellationToken cancellationToken);
    }
}
