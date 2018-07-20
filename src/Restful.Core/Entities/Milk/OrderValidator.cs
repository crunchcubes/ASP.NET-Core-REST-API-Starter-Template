using Restful.Core.Interfaces.Milk;
using System.Collections.Generic;
using System.Linq;

namespace Restful.Core.Entities.Milk
{
    public class OrderValidator : IOrderValidator
    {
        public OrderResult Validate(int quantity, Product product)
        {
            if (quantity == 0)
            {
                return OrderResult.Rejected;
            }

            if (product.OrderUnit == OrderUnit.ByBox)
            {
                if (quantity >= product.QuantityPerBox && quantity % product.QuantityPerBox == 0)
                {
                    if (quantity >= product.MinimumOrderUnitQuantity * product.QuantityPerBox)
                    {
                        return OrderResult.Accepted;
                    }
                }
            }
            else
            {
                if (quantity >= product.MinimumOrderUnitQuantity)
                {
                    return OrderResult.Accepted;
                }
            }
            return OrderResult.Rejected;
        }

        public OrderResult Validate(IEnumerable<(Product product, int quantity)> orders)
        {
            var valueTuples = orders as (Product product, int quantity)[] ?? orders.ToArray();

            if (valueTuples.Any() && valueTuples.All(x => Validate(x.quantity, x.product) == OrderResult.Accepted))
            {
                return OrderResult.Accepted;
            }
            return OrderResult.Rejected;
        }
    }
}
