using FluentValidation;
using RentalCar.Categories.Application.Commands.Request;

namespace RentalCar.Categories.Application.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator() 
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Informe o código");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Informe o nome");

            RuleFor(c => c.DialyPrice)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero (0)");
        }
    }
}
