using System.Net;
using MediatR;
using RentalCar.Categories.Application.Queries.Request.Categories;
using RentalCar.Categories.Application.Queries.Response.Categories;
using RentalCar.Categories.Application.Wrappers;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;

namespace RentalCar.Categories.Application.Handlers.Categories
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
                var results = new List<FindCategoryResponse>();
                var categories = await _categoryRepository.GetAll(cancellationToken);

                results = categories.Select(category => new FindCategoryResponse(category.Id, category.Name, category.DailyPrice)).ToList();
                
                _prometheusService.AddCategoryCounter(HttpStatusCode.OK.ToString());
                
                return new PagedResponse<FindCategoryResponse>(results, MensagemError.CarregamentoSucesso(Objecto, results.Count));
            }
            catch (Exception ex)
            {
                _prometheusService.AddCategoryCounter(HttpStatusCode.BadRequest.ToString());
                _loggerService.LogError(MensagemError.CarregamentoErro(Objecto, ex.Message));
                return new PagedResponse<FindCategoryResponse>(MensagemError.CarregamentoErro(Objecto));
                //throw;
            }
        }

    }
}
