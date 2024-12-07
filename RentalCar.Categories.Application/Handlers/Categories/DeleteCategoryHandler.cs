using System.Net;
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
        private readonly IPrometheusService _prometheusService;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository, ILoggerService loggerService, IPrometheusService prometheusService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
            _prometheusService = prometheusService;
        }

        public async Task<ApiResponse<InputCategoryResponse>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
        {
            const string objecto = "categoria";
            const string operacao = "eliminar"; 
            try
            {
                var category = await _categoryRepository.GetById(request.Id, cancellationToken);
                if (category == null)
                {
                    _prometheusService.AddCategoryCounter(HttpStatusCode.NotFound.ToString());
                    _loggerService.LogWarning(MensagemError.NotFound(objecto, request.Id));
                    return ApiResponse<InputCategoryResponse>.Error(MensagemError.NotFound(objecto));
                }

                await _categoryRepository.Delete(category, cancellationToken);

                var result = new InputCategoryResponse(category.Id, category.Name, category.DailyPrice);
                _prometheusService.AddCategoryCounter(HttpStatusCode.OK.ToString());
                return ApiResponse<InputCategoryResponse>.Success(result, MensagemError.OperacaoSucesso(objecto, operacao));
            }
            catch (Exception ex)
            {
                _prometheusService.AddCategoryCounter(HttpStatusCode.BadRequest.ToString());
                _loggerService.LogError(MensagemError.OperacaoErro(objecto, operacao, ex.Message));
                return ApiResponse<InputCategoryResponse>.Error(MensagemError.OperacaoErro(objecto, operacao));
                //throw;
            }
        }
    }
}
