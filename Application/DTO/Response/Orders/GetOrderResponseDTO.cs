using Application.DTO.Request.Orders;
using MediatR.NotificationPublishers;
using System.Globalization;

namespace Application.DTO.Response.Orders
{
    public class GetOrderResponseDTO : OrderBaseDTO
    {
        public Guid Id { get; set; }
        public string State { get; set; }
        public string ProductName { get; set; }
        public string SerialNumber { get; set; }
        public DateTime OrderedDate { get; set; }
        public DateTime DeliveringDate { get; set; }
        public String ProductImage { get; set; }
        public string PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public string PersonnelService { get; set; }
        public string Personnelsociety {get; set;}
    }
}
