﻿using System.Net;
using MediatR;
using RentalCar.Categories.Application.Queries.Request;
using RentalCar.Categories.Application.Queries.Response;
using RentalCar.Categories.Application.Utils;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Handlers
{
    public class FindCategoryByIdHandler : IRequestHandler<FindCategoryByIdRequest, ApiResponse<FindCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;
        private readonly IPrometheusService _prometheusService;

        public FindCategoryByIdHandler(ICategoryRepository categoryRepository, ILoggerService loggerService, IPrometheusService prometheusService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
            _prometheusService = prometheusService;
        }

        public async Task<ApiResponse<FindCategoryResponse>> Handle(FindCategoryByIdRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "categoria";
            try
            {
                var category = await _categoryRepository.GetById(request.Id, cancellationToken);
                if (category == null) 
                {
                    _prometheusService.AddFindByIdCategoryCounter(HttpStatusCode.NotFound.ToString());
                    _loggerService.LogWarning(MensagemError.NotFound(Objecto, request.Id));
                    return ApiResponse<FindCategoryResponse>.Error(MensagemError.NotFound(Objecto));
                }

                var result = new FindCategoryResponse(category.Id, category.Name, category.DailyPrice);
                _prometheusService.AddFindByIdCategoryCounter(HttpStatusCode.OK.ToString());
                _loggerService.LogInformation(MensagemError.CarregamentoSucesso(Objecto, 1));
                return ApiResponse<FindCategoryResponse>.Success(result, MensagemError.CarregamentoSucesso(Objecto));
            }
            catch (Exception ex)
            {
                _prometheusService.AddFindByIdCategoryCounter(HttpStatusCode.BadRequest.ToString());
                _loggerService.LogError(MensagemError.CarregamentoErro(Objecto, ex.Message));
                return ApiResponse<FindCategoryResponse>.Error(MensagemError.CarregamentoErro(Objecto));
                //throw;
            }
        }

    }
}
