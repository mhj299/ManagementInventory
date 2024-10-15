using Application.DTO.Response.Orders;
using MediatR;


namespace Application.Service.Orders.Queries
{
   public record  GetAllOrderQuery: IRequest<IEnumerable<GetOrderResponseDTO>>;
    
}
