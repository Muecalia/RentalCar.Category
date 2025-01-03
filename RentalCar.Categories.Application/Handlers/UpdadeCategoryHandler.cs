﻿using System.Net;
using MediatR;
using RentalCar.Categories.Application.Commands.Request;
using RentalCar.Categories.Application.Commands.Response;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;
using RentalCar.Categories.Core.Wrappers;

namespace RentalCar.Categories.Application.Handlers
{
    public class UpdadeCategoryHandler : IRequestHandler<UpdateCategoryRequest, ApiResponse<InputCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;
        private readonly IPrometheusService _prometheusService;

        public UpdadeCategoryHandler(ICategoryRepository categoryRepository, ILoggerService loggerService, IPrometheusService prometheusService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
            _prometheusService = prometheusService;
        }

        public async Task<ApiResponse<InputCategoryResponse>> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            const string Objecto = "employeee";
            const string Operacao = "editar";
            try
            {
                var category = await _categoryRepository.GetById(request.Id, cancellationToken);
                if (category == null) 
                {
                    _prometheusService.AddUpdateCategoryCounter(HttpStatusCode.NotFound.ToString());
                    _loggerService.LogWarning(MensagemError.NotFound(Objecto, request.Id));
                    return ApiResponse<InputCategoryResponse>.Error(MensagemError.NotFound(Objecto));
                }

                category.Name = request.Name;
                category.DailyPrice = request.DialyPrice;

                await _categoryRepository.Update(category, cancellationToken);

                var result = new InputCategoryResponse(category.Id, category.Name, category.DailyPrice);
                _prometheusService.AddUpdateCategoryCounter(HttpStatusCode.OK.ToString());
                return ApiResponse<InputCategoryResponse>.Success(result, MensagemError.OperacaoSucesso(Objecto, Operacao));
            }
            catch (Exception ex)
            {
                _prometheusService.AddUpdateCategoryCounter(HttpStatusCode.BadRequest.ToString());
                _loggerService.LogError(MensagemError.OperacaoErro(Objecto, Operacao, ex.Message));
                return ApiResponse<InputCategoryResponse>.Error(MensagemError.OperacaoErro(Objecto, Operacao));
                //throw;
            }
        }
    }
}
