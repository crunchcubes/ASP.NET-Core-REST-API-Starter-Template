using System;
using System.Collections.Generic;
using Restful.Core.Entities.Milk;
using Restful.Infrastructure.Services;

namespace Restful.Infrastructure.Resources.Milk.PropertyMappings
{
    public class ProductPropertyMapping : PropertyMapping<ProductResource, Product>
    {
        public ProductPropertyMapping() : base(new Dictionary<string, List<MappedProperty>>
               (StringComparer.OrdinalIgnoreCase)
        {
            [nameof(ProductResource.Name)] = new List<MappedProperty>
            {
                new MappedProperty{ Name = nameof(Product.Name), Revert = false}
            },
            [nameof(ProductResource.UnitPrice)] = new List<MappedProperty>
            {
                new MappedProperty{ Name = nameof(Product.UnitPrice), Revert = false}
            },
            [nameof(ProductResource.QuantityPerBox)] = new List<MappedProperty>
            {
                new MappedProperty{ Name = nameof(Product.QuantityPerBox), Revert = false}
            }
        })
        {
        }
    }
}
