using Restful.Core.Interfaces;

namespace Restful.Core.Entities.Milk
{
    public class Product: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PackingType PackingType { get; set; }
        public OrderUnit OrderUnit { get; set; }
        public int QuantityPerBox { get; set; }
        public int MinimumOrderUnitQuantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
