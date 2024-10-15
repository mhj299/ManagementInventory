
namespace Application.DTO.Request.Orders
{
   public class UpdatePersonnelOrderRequestDTO
    {
        public Guid OrderId { get; set; }
        public string OrderState { get; set; }
        public DateTime DeliveringDate { get; set; } = DateTime.Now;


    }
}
