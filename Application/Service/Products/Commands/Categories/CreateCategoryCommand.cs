using Application.DTO.Request.Products;
using Application.DTO.Response;
using MediatR;

namespace Application.Service.Products.Commands.Categories
{
    public record DeleteCategoryCommand(AddCategoryRequestDTO CategoryModel) : IRequest<ServiceResponse>
    {
       
    }
}
