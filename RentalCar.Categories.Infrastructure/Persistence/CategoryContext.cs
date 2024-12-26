using Microsoft.EntityFrameworkCore;
using RentalCar.Categories.Core.Entities;

namespace RentalCar.Categories.Infrastructure.Persistence;

public class CategoryContext : DbContext
{
    public CategoryContext(DbContextOptions<CategoryContext> options) : base(options)
    {
    }

    public DbSet<Category> Category { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>(e =>
        {
            e.HasKey(c => c.Id);

            e.Property<string>(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            
            e.HasIndex(c => c.Name).IsUnique();
        });
    }
}