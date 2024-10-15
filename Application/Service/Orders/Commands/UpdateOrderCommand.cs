using Application.DTO.Request.Orders;
using Application.DTO.Response;
using MediatR;


namespace Application.Service.Orders.Commands
{
    public record UpdatePersonnelOrderCommand(UpdatePersonnelOrderRequestDTO model) : IRequest<ServiceResponse>
    {
        public object Model { get; set; }
    }
}
