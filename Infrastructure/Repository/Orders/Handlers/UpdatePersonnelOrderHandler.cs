using Application.DTO.Response;
using Application.Service.Orders.Commands;
using Infrastructure.DataAccess;
using MediatR;

namespace Infrastructure.Repository.Orders.Handlers
{
    public class UpdatePersonnelOrderHandler : IRequestHandler<UpdatePersonnelOrderCommand, ServiceResponse>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public UpdatePersonnelOrderHandler(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<ServiceResponse> Handle(UpdatePersonnelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = _contextFactory.CreateDbContext();
                var data = await dbContext.Orders  .Where(_ => _.Id == request.Model.OrderId)
                         .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (data == null)
                    return new ServiceResponse(false, "Order not found");

                data.OrderState = request.Model.OrderState;
                data.DeliveringDate = request.Model.DeliveringDate;
                await dbContext.SaveChangesAsync(cancellationToken);

                return new ServiceResponse(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message); 
            }
        }
    }
}
