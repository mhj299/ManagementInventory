using Application.DTO.Response;
using Application.Service.Orders.Commands;
using Application.Service.Products.Commands.Categories;
using Domain.Entities.Orders;
using Infrastructure.DataAccess;
using Infrastructure.Repository.Products;
using Mapster;
using MediatR;
using Application.Extension;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repository.Orders.Handlers
{
   public  class CreateOrderHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<CreateOrderCommand, ServiceResponse>
    {
        public enum OrderState
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Canceled,
            Delivereing,
            Delivering
        }
        public class Order
        {
            public OrderState OrderState { get; set; }
            // Autres propriétés de l'ordre
        }

        public async Task<ServiceResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();
                var product = await dbContext.Products.FirstOrDefaultAsync(_ => _.Id==request.Model.ProductId, cancellationToken: cancellationToken);
                var data = request.Model.Adapt<Order>();
                data.OrderState = OrderState.Processing;
                dbContext.Orders.Add(data) ;

                await dbContext.SaveChangesAsync(cancellationToken);
                return new ServiceResponse(true,"order placed successfully"); 

            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}

