using MediatR;
using RentalCar.Categories.Application.Commands.Request.Categories;
using RentalCar.Categories.Application.Commands.Response.Categories;
using RentalCar.Categories.Application.Wrappers;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;

namespace RentalCar.Categories.Application.Handlers.Categories
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryRequest, ApiResponse<InputCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository, ILoggerService loggerService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
        }

        public async Task<ApiResponse<InputCategoryResponse>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "categoria";
            const string Operacao = "eliminar";
            try
            {
                var category = await _categoryRepository.GetById(request.Id, cancellationToken);
                if (category == null)
                {
                    _loggerService.LogWarning(MensagemError.NotFound(Objecto, request.Id));
                    return ApiResponse<InputCategoryResponse>.Error(MensagemError.NotFound(Objecto));
                }

                await _categoryRepository.Delete(category, cancellationToken);

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
