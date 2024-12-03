using MediatR;
using RentalCar.Categories.Application.Commands.Request.Categories;
using RentalCar.Categories.Application.Commands.Response.Categories;
using RentalCar.Categories.Application.Wrappers;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;

namespace RentalCar.Categories.Application.Handlers.Categories
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, ApiResponse<InputCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, ILoggerService loggerService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
        }

        public async Task<ApiResponse<InputCategoryResponse>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "employeee";
            const string Operacao = "criação";
            try
            {
                if (await _categoryRepository.IsCategoryExist(request.Name, cancellationToken))
                {
                    _loggerService.LogWarning(MensagemError.Conflito($"{Objecto} {request.Name}"));
                    return ApiResponse<InputCategoryResponse>.Error(MensagemError.Conflito(Objecto));
                }

                var newCategory = new Category
                {
                    Name = request.Name,
                    DailyPrice = request.DialyPrice
                };

                var category = await _categoryRepository.Create(newCategory, cancellationToken);

                var result = new InputCategoryResponse(category.Id, category.Name, category.DailyPrice);

                return ApiResponse<InputCategoryResponse>.Success(result, MensagemError.OperacaoSucesso(Objecto, Operacao));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(MensagemError.OperacaoErro(Objecto, Operacao, ex.Message));
                return ApiResponse<InputCategoryResponse>.Error(MensagemError.OperacaoErro(Objecto, Operacao));
                //throw;
            }
        }

    }
}
