﻿using Application.DTO.Response;
using Application.Service.Products.Commands.Locations;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.Locations
{
    public  class CreateLocationHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory): IRequestHandler<CreateLocationCommand,ServiceResponse>
    {
        public async Task<ServiceResponse>Handle(CreateLocationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var location = await dbContext.Locations.FirstOrDefaultAsync(_ => _.Name.ToLower().Equals(request.LocationModel.Name.ToLower()), cancellationToken: cancellationToken);
                if (location != null)
                    return GeneralDbResponses.ItemAlreadyExist(request.LocationModel.Name);
                var data = request.LocationModel.Adapt(new Location());
                dbContext.Locations.Add(data);
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemCreated(request.LocationModel.Name);


            }
            catch(Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}
