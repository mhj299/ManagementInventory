using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities.Products
{
    public  class Category: ProductBase
    {
        [JsonIgnore]// L'attribut [JsonIgnore] est utilisé pour éviter que la collection Products ne soit sérialisée en JSON,
        public virtual ICollection<Product> Products { get; set; } = null;

    }
}
