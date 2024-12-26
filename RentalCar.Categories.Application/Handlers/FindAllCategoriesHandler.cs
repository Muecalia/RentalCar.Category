using System.Net;
using MediatR;
using RentalCar.Categories.Application.Queries.Request;
using RentalCar.Categories.Application.Queries.Response;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Handlers
{
    public class FindAllCategoriesHandler : IRequestHandler<FindAllCategoriesRequest, PagedResponse<FindCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;
        private readonly IPrometheusService _prometheusService;

        public FindAllCategoriesHandler(ICategoryRepository categoryRepository, ILoggerService loggerService, IPrometheusService prometheusService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
            _prometheusService = prometheusService;
        }

        public async Task<PagedResponse<FindCategoryResponse>> Handle(FindAllCategoriesRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "categorias";
            try
            {
                var categories = await _categoryRepository.GetAll(request.PageNumber, request.PageSize, cancellationToken);

                var results = categories.Select(category => new FindCategoryResponse(category.Id, category.Name, category.DailyPrice)).ToList();
                
                _prometheusService.AddFindAllCategorysCounter(HttpStatusCode.OK.ToString());
                
                return new PagedResponse<FindCategoryResponse>(results, request.PageNumber, request.PageSize, results.Count, MensagemError.CarregamentoSucesso(Objecto, results.Count));
            }
            catch (Exception ex)
            {
                _prometheusService.AddFindAllCategorysCounter(HttpStatusCode.BadRequest.ToString());
                _loggerService.LogError(MensagemError.CarregamentoErro(Objecto, ex.Message));
                return new PagedResponse<FindCategoryResponse>(MensagemError.CarregamentoErro(Objecto));
                //throw;
            }
        }

    }
}
