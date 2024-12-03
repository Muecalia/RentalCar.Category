﻿using MediatR;
using RentalCar.Categories.Application.Commands.Request.Categories;
using RentalCar.Categories.Application.Commands.Response.Categories;
using RentalCar.Categories.Application.Wrappers;
using RentalCar.Categories.Core.Repositories;
using RentalCar.Categories.Core.Services;
using RentalCar.Categories.Application.Utils;

namespace RentalCar.Categories.Application.Handlers.Categories
{
    public class UpdadeCategoryHandler : IRequestHandler<UpdateCategoryRequest, ApiResponse<InputCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILoggerService _loggerService;

        public UpdadeCategoryHandler(ICategoryRepository categoryRepository, ILoggerService loggerService)
        {
            _categoryRepository = categoryRepository;
            _loggerService = loggerService;
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
                    _loggerService.LogWarning(MensagemError.NotFound(Objecto, request.Id));
                    return ApiResponse<InputCategoryResponse>.Error(MensagemError.NotFound(Objecto));
                }

                category.Name = request.Name;
                category.DailyPrice = request.DialyPrice;

                await _categoryRepository.Update(category, cancellationToken);

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