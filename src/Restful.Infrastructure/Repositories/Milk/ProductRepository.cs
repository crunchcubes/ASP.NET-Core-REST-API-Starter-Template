using Microsoft.EntityFrameworkCore;
using Restful.Core.Entities.Milk;
using Restful.Core.Helpers;
using Restful.Core.Interfaces.Milk;
using Restful.Infrastructure.Database;
using Restful.Infrastructure.Extensions;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Services;
using System.Linq;
using System.Threading.Tasks;
using Restful.Core.Helpers.Milk;

namespace Restful.Infrastructure.Repositories.Milk
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyContext _myContext;
        private readonly IPropertyMappingContainer _propertyMappingContainer;

        public ProductRepository(MyContext myContext, IPropertyMappingContainer propertyMappingContainer)
        {
            _myContext = myContext;
            _propertyMappingContainer = propertyMappingContainer;
        }

        public async Task<PaginatedList<Product>> GetProductsAsync(ProductResourceParameters parameters)
        {
            var query = _myContext.Products.AsQueryable();

            if (!string.IsNullOrEmpty(parameters.Name))
            {
                var name = parameters.Name.Trim().ToLowerInvariant();
                query = query.Where(x => x.Name.ToLowerInvariant() == name);
            }

            query = query.ApplySort(parameters.OrderBy, _propertyMappingContainer.Resolve<ProductResource, Product>());

            var count = await query.CountAsync();
            var items = await query
                .Skip(parameters.PageSize * parameters.PageIndex)
                .Take(parameters.PageSize).ToListAsync();

            return new PaginatedList<Product>(parameters.PageIndex, parameters.PageSize, count, items);
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _myContext.Products.FindAsync(id);
        }

        public void AddProduct(Product product)
        {
            _myContext.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _myContext.Products.Update(product);
        }

        public void DeleteProduct(Product product)
        {
            _myContext.Products.Remove(product);
        }
    }
}
