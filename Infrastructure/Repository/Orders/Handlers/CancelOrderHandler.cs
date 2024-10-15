using Application.DTO.Response;
using Application.Service.Orders.Commands;
using Application.Service.Products.Commands.Categories;
using Infrastructure.DataAccess;
using Infrastructure.Repository.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Infrastructure.Repository.Orders.Handlers.CreateOrderHandler;


namespace Infrastructure.Repository.Orders.Handlers
{
    public class CancelOrderHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory): IRequestHandler<CancelOrderCommand,ServiceResponse>

    {
        
        
        public async Task<ServiceResponse> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();//Le handler utilise une fabrique de contexte de base de données (IDbContextFactory<AppDbContext>) pour créer un contexte (DbContext) nécessaire pour interagir avec la base de données.
                var order = await dbContext.Orders.Where(_ => _.Id==request.OrderId).FirstOrDefaultAsync(cancellationToken: cancellationToken); //FirstOrDefaultAsync : Recherchez la première catégorie qui correspond à l'ID fourni ou renvoyez null si aucune correspondance n'est trouvée.
                if (order== null)
                    return new ServiceResponse(false,"Order not found");//Si la catégorie n'est pas trouvée, le handler renvoie une réponse indiquant que l'élément n'a pas été trouvé.

                order.OrderState = OrderState.Canceled.ToString();//Si la catégorie existe, elle est supprimée de la base de données.
                await dbContext.SaveChangesAsync(cancellationToken);
                return new ServiceResponse(true, "Order canceled successfully"); //Après la suppression, une réponse est renvoyée pour indiquer que la catégorie a été supprimée avec succès.

            }
            catch (Exception ex)//Si une exception se produit pendant l'opération, elle est capturée, et une réponse d'erreur est renvoyée.
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}

