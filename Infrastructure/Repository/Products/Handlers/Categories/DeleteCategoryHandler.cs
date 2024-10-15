using Application.DTO.Response;
using Application.Service.Products.Commands.Categories;
using Domain.Entities.Products;
using Infrastructure.DataAccess;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;


namespace Infrastructure.Repository.Products.Handlers.Categories
{
    public class DeleteCategoryHandler(DataAccess.IDbContextFactory<AppDbContext> contextFactory) : IRequestHandler<DeleteCategroyCommand, ServiceResponse>

    //IRequestHandler<DeleteCategoryCommand, ServiceResponse> : Indique que ce handler gère la commande DeleteCategoryCommand et retourne une réponse de type ServiceResponse.
    {
        public async Task<ServiceResponse> Handle(DeleteCategroyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                using var dbContext = contextFactory.CreateDbContext();//Le handler utilise une fabrique de contexte de base de données (IDbContextFactory<AppDbContext>) pour créer un contexte (DbContext) nécessaire pour interagir avec la base de données.
                var data = await dbContext.Categories.FirstOrDefaultAsync(_ => _.Id.Equals(request.Id), cancellationToken: cancellationToken); //FirstOrDefaultAsync : Recherchez la première catégorie qui correspond à l'ID fourni ou renvoyez null si aucune correspondance n'est trouvée.
                if (data == null)
                    return GeneralDbResponses.ItemNotFound("Category");//Si la catégorie n'est pas trouvée, le handler renvoie une réponse indiquant que l'élément n'a pas été trouvé.

                dbContext.Categories.Remove(data);//Si la catégorie existe, elle est supprimée de la base de données.
                await dbContext.SaveChangesAsync(cancellationToken);
                return GeneralDbResponses.ItemDelete(data.Name); //Après la suppression, une réponse est renvoyée pour indiquer que la catégorie a été supprimée avec succès.

            }
            catch (Exception ex)//Si une exception se produit pendant l'opération, elle est capturée, et une réponse d'erreur est renvoyée.
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
    }
}
