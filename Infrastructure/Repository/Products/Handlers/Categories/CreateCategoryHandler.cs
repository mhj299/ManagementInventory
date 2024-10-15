﻿using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repository.Products.Handlers.Categories
{
    public  class CreateCategoryHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory): IRequestHandler<DeleteCategoryCommand, ServiceResponse>

    {
        public async Task<ServiceResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var category = await dbContext.Categories.FirstOrDefaultAsync(_ => _.Name.ToLower().Equals(request.CategoryModel.Name.ToLower()), cancellationToken: cancellationToken);
                if (category != null)
                    return GeneralDbResponses.ItemAlreadyExist(request.CategoryModel.Name);
                var data = request.CategoryModel.Adapt(new Category());
                dbContext.Categories.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemCreated(request.CategoryModel.Name);

            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}
