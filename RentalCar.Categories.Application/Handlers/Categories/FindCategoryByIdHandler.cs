using MediatR;
using RentalCar.Categories.Application.Queries.Request.Categories;
using RentalCar.Categories.Application.Queries.Response.Categories;
using RentalCar.Categories.Application.Utils;
using RentalCar.Categories.Application.Wrappers;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;

namespace RentalCar.Categories.Application.Handlers.Categories
{
    public class FindCategoryByIdHandler : IRequestHandler<FindCategoryByIdRequest, ApiResponse<FindCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;

        public FindCategoryByIdHandler(ICategoryRepository categoryRepository, ILoggerService loggerService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
        }

        public async Task<ApiResponse<FindCategoryResponse>> Handle(FindCategoryByIdRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "categoria";
            try
            {
                var category = await _categoryRepository.GetById(request.Id, cancellationToken);
                if (category == null) 
                {
                    _loggerService.LogWarning(MensagemError.NotFound(Objecto, request.Id));
                    return ApiResponse<FindCategoryResponse>.Error(MensagemError.NotFound(Objecto));
                }

                var result = new FindCategoryResponse(category.Id, category.Name, category.DailyPrice);

                _loggerService.LogInformation(MensagemError.CarregamentoSucesso(Objecto, 1));
                return ApiResponse<FindCategoryResponse>.Success(result, MensagemError.CarregamentoSucesso(Objecto));
            }
            catch (Exception ex)
            {
                _loggerService.LogError(MensagemError.CarregamentoErro(Objecto, ex.Message));
                return ApiResponse<FindCategoryResponse>.Error(MensagemError.CarregamentoErro(Objecto));
                //throw;
            }
        }

    }
}
