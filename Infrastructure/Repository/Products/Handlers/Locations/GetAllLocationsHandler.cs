﻿using Application.DTO.Response.Products;
using Application.Service.Products.Queries.Categories;
using Application.Service.Products.Queries.Locations;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.Locations
{
    public class GetAllLocationsHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetAllLocationsQuery, IEnumerable<GetLocationResponseDTO>>

    {
        public async Task<IEnumerable<GetLocationResponseDTO>> Handle(GetAllLocationsQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var data = await dbContext.Categories.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            return data.Adapt<List<GetLocationResponseDTO>>();

        }

    }
}
