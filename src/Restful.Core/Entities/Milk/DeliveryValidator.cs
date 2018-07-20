using Restful.Core.Interfaces.Milk;
using System.Collections.Generic;
using System.Linq;

namespace Restful.Core.Entities.Milk
{
    public class DeliveryValidator : IDeliveryValidator
    {
        private const int BoxArea = 100;
        private const int SingleArea = 20;
        private readonly IOrderValidator _orderValidator;

        public DeliveryValidator(IOrderValidator orderValidator)
        {
            _orderValidator = orderValidator;
        }

        public DeliveryResult ValidateByTruck(IEnumerable<(Product product, int quantity)> orders)
        {
            var valueTuples = orders as (Product product, int quantity)[] ?? orders.ToArray();

            if (_orderValidator.Validate(valueTuples) == OrderResult.Accepted)
            {
                var byBox = valueTuples.Where(x => x.product.OrderUnit == OrderUnit.ByBox).ToList();
                var byBoxTotal = byBox.Sum(x => x.quantity * (int)x.product.PackingType);

                if (byBoxTotal <= BoxArea)
                {
                    var byOne = valueTuples.Where(x => x.product.OrderUnit == OrderUnit.ByOne).ToList();
                    var byOneTotal = byOne.Sum(x => x.quantity * (int)x.product.PackingType);

                    if (byOneTotal <= SingleArea)
                    {
                        return DeliveryResult.Accepted;
                    }

                    var byOneLeft = byOneTotal - SingleArea;
                    var boxAreaLeftSpace = BoxArea - byBoxTotal;
                    if (byOneLeft <= boxAreaLeftSpace)
                    {
                        return DeliveryResult.Accepted;
                    }
                    return DeliveryResult.PartialAccepted;
                }
            }

            return DeliveryResult.Rejected;
        }
    }
}
