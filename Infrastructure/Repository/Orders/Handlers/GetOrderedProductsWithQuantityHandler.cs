using Application.DTO.Response.Orders;
using Application.Service.Orders.Queries;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class GetOrderedProductsWithQuantityHandler : IRequestHandler<GetOrderedProductsWithQuantityQuery, IEnumerable<GetOrderedProductsWithQuantityResponseDTO>>
    {
        private readonly DataAccess.IDbContextFactory<AppDbContext> _contextFactory;

        public GetOrderedProductsWithQuantityHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<GetOrderedProductsWithQuantityResponseDTO>> Handle(GetOrderedProductsWithQuantityQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            var orders = new List<Order>();
            var data = new List<GetOrderedProductsWithQuantityResponseDTO>();

            if (!string.IsNullOrEmpty(request.UserId))
            {
                orders = await dbContext.Orders.AsNoTracking()
                    .Where(o => o.ClientId.ToString() == request.UserId)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
            else
            {
                orders = await dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            }

            var selectOrdersWithin12Months = orders
                .Where(order => order.DateOrdered.Date >= DateTime.Today.AddMonths(-12))
                .GroupBy(order => order.ProductId)
                .ToList();

            foreach (var orderGroup in selectOrdersWithin12Months)
            {
                var product = await dbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == orderGroup.Key, cancellationToken: cancellationToken);

                if (product != null)
                {
                    data.Add(new GetOrderedProductsWithQuantityResponseDTO()
                    {
                        ProductName = product.Name,
                        QuantityOrdered = orderGroup.Sum(x => x.Quantity)
                    });
                }
            }

            return data;
        }
    }
}
