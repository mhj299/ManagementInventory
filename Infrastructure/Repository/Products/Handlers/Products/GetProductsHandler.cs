using Application.DTO.Response.Orders;
using Application.DTO.Response.Products;
using Application.Service.Products.Queries.Products;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Infrastructure.Repository.Products.Handlers.Locations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.Products
{
    public class GetProductsHandler : IRequestHandler<GetProductQuery, IEnumerable<GetProductResponseDTO>>
    {
        private readonly DataAccess.IDbContextFactory<AppDbContext> _contextFactory;

        public GetProductsHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<GetProductResponseDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            using var context = _contextFactory.CreateDbContext();
            var products = await context.Products
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(l => l.Location)
                .ToListAsync(cancellationToken: cancellationToken);

            return products.Select(product => new GetProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Base64Image = product.Base64Image,
                CategoryId = product.CategoryId,
                LocationId = product.LocationId,
                DateAdded = product.DateAdded,
                Quantity = product.Quantity,
                SerialNumber = product.SerialNumber,
                Location = new GetLocationResponseDTO
                {
                    Id = product.LocationId,
                    Name = product.Location.Name
                },
                Category = new GetCategoryResponseDTO
                {
                    Id = product.CategoryId,
                    Name = product.Category.Name,
                }
            }).ToList();
        }
    }
}
