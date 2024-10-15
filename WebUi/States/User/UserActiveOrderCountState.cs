using Application.Service.Orders.Queries;
using MediatR;
using System.ComponentModel.Design;

namespace WebUi.States.User
{
    public class UserActiveOrderCountState
    {
        public int ProcessingCount { get; set; }
        public int DeliveringCount { get; set; }
        public int DeliveredCount { get; set; }
        public int CanceledCount { get; set; }
       public string UserId { get; private set; }

        public event Action? StateChanged;
        public async Task GetActiveOrdersCount (string userId)
        {
            using var scope = ServiceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var response = (await mediator.Send(new GetGenericOrdersCountQuery(UserId, false)));
            ProcessingCount = response.Processing();
            DeliveringCount = response.Delivering();
            DeliveredCount = response.Delivered();
            CanceledCount = response.Canceled();
            StateChanged?.Invoke();
        }
    }
}
