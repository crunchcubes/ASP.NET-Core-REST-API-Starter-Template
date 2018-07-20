using Restful.Core.Entities.Milk;

namespace Restful.Core.Interfaces.Milk
{
    public interface IDeliveryValidator
    {
        DeliveryResult ValidateByTruck(System.Collections.Generic.IEnumerable<(Product product, int quantity)> orders);
    }
}