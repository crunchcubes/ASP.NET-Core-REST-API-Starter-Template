using Restful.Core.Entities.Milk;

namespace Restful.Infrastructure.Resources.Milk
{
    public class ProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PackingType PackingType { get; set; }
        public OrderUnit OrderUnit { get; set; }
        public int QuantityPerBox { get; set; }
        public int MinimumOrderQuantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
