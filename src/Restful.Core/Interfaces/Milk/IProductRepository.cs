using Restful.Core.Entities.Milk;
using Restful.Core.Helpers;
using System.Threading.Tasks;
using Restful.Core.Helpers.Milk;

namespace Restful.Core.Interfaces.Milk
{
    public interface IProductRepository
    {
        Task<PaginatedList<Product>> GetProductsAsync(ProductResourceParameters parameters);
        Task<Product> GetProductByIdAsync(int id);
        void AddProduct(Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
    }
}
