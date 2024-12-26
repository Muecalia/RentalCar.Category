using FluentValidation;
using RentalCar.Categories.Application.Commands.Request;

namespace RentalCar.Categories.Application.Validators
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
