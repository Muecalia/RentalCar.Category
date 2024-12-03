using FluentValidation;
using RentalCar.Categories.Application.Commands.Request.Categories;

namespace RentalCar.Categories.Application.Validators.Categories
{
    public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryRequest>
    {
        public DeleteCategoryValidator() 
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Informe o código");
        }
    }
}
