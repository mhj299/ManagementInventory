﻿using Application.DTO.Response.Orders;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class GetProductOrderedByMonthsHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<GetProductOrderedByMonthsQuery, IEnumerable<GetProductOrderedByMonthsResponseDTO>>

    {
        public async Task<IEnumerable<GetProductOrderedByMonthsResponseDTO>> Handle(GetProductOrderedByMonthsQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = contextFactory.CreateDbContext();
            var orders = new List<Order>();
            var data = new List<GetProductOrderedByMonthsResponseDTO>();
            if (!string.IsNullOrEmpty(request.UserId))
                orders = await dbContext.Orders.AsNoTracking().Where(_ => _.ClientId.ToString() == request.UserId).ToListAsync(cancellationToken: cancellationToken);
            else
                orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            var selectOrdersWithin12Months = orders
                                           .Where(order => order.DateOrdered >= DateTime.Today.AddMonths(-12))
                                           .GroupBy(order => new { Month = order.DateOrdered.Month })
                                           .ToList();
            foreach (var order in selectOrdersWithin12Months.OrderBy(_ => _.Key.Month))
            {
                data.Add(new GetProductOrderedByMonthsResponseDTO()
                {
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(order.Key.Month),
                   
                });

            }
            return data;
        }
    }
}

