using Microsoft.EntityFrameworkCore;
using RentalCar.Categories.Core.Entities;

namespace RentalCar.Categories.Infrastructure.Persistence;

public class RentalCarCategoryContext : DbContext
{
    public RentalCarCategoryContext(DbContextOptions<RentalCarCategoryContext> options) : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);
            e.HasIndex(c => c.Name);
        });
    }
}