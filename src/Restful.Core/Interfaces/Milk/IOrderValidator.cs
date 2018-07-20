using Restful.Core.Entities.Milk;
using System.Collections.Generic;

namespace Restful.Core.Interfaces.Milk
{
    public interface IOrderValidator
    {
        OrderResult Validate(int quantity, Product product);
        OrderResult Validate(IEnumerable<(Product product, int quantity)> orders);
    }
}