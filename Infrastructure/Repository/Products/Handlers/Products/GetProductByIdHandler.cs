using Application.DTO.Response.Products;
using Application.Service.Products.Queries.Products;
using Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Products.Handlers.Products
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, GetProductResponseDTO>
    {
        private readonly DataAccess.IDbContextFactory<AppDbContext> _contextFactory;

        public GetProductByIdHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<GetProductResponseDTO> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            using var dbContext = _contextFactory.CreateDbContext();
            var product = await dbContext.Products
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(l => l.Location)  // Corrected capitalization
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return null; // Handle the case when the product is not found (optional)
            }

            return new GetProductResponseDTO
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
            };
        }
    }
}
