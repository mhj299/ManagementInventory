﻿using Application.DTO.Request.Products;
using Application.DTO.Response;
using MediatR;


namespace Application.Service.Products.Commands.Locations
{
    public record CreateLocationCommand (AddLocationRequestDTO LocationModel):IRequest<ServiceResponse>;
   
}
