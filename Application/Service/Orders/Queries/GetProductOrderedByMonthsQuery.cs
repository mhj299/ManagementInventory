﻿using Application.DTO.Response.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Orders.Queries
{
   public record  GetProductOrderedByMonthsQuery(string UserId=null):IRequest<IEnumerable<GetProductOrderedByMonthsResponseDTO>>;
   
}
