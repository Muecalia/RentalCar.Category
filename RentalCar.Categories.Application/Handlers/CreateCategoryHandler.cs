using System.Net;
using MediatR;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Application.Commands.Response;
using RentalCar.Categories.Core.Entities;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, ApiResponse<InputCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;
        private readonly IPrometheusService _prometheusService;

        public CreateCategoryHandler(ICategoryRepository categoryRepository, ILoggerService loggerService, IPrometheusService prometheusService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
            _prometheusService = prometheusService;
        }

        public async Task<ApiResponse<InputCategoryResponse>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "categoria";
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
                _prometheusService.AddNewCategoryCounter(HttpStatusCode.Created.ToString());
                return ApiResponse<InputCategoryResponse>.Success(result, MensagemError.OperacaoSucesso(Objecto, Operacao));
            }
            catch (Exception ex)
            {
                _prometheusService.AddNewCategoryCounter(HttpStatusCode.BadRequest.ToString());
                _loggerService.LogError(MensagemError.OperacaoErro(Objecto, Operacao, ex.Message));
                return ApiResponse<InputCategoryResponse>.Error(MensagemError.OperacaoErro(Objecto, Operacao));
                //throw;
            }
        }

    }
}
