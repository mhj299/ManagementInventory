
using System.ComponentModel.DataAnnotations;
namespace Domain.Entities.Products
{
   
    public  class ProductBase

    {
        [Key] // pour indiquer que le proprieté id est la clé primaire de l'entité
        public Guid Id { get; set; }  = Guid.NewGuid(); // Cela garantit qu'un nouvel identifiant unique est généré chaque fois qu'une nouvelle instance de ProductBase est créée.
        public string Name { get; set; }

    }
}
