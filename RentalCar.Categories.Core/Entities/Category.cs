using System.ComponentModel.DataAnnotations;

namespace RentalCar.Categories.Core.Entities;

public class Category
{
   public Category()
    {
        IsDeleted = false;
        Id = Guid.NewGuid().ToString();
        CreatedAt = DateTime.Now;
    }

    [MaxLength(50)]
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    [MaxLength(50)]
    public required string Name { get; set; }
    public decimal DailyPrice { get; set; } 
}