using Restful.Core.Entities.Milk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    public static class Products
    {
        public static readonly Product PasteurizedMilk = new Product
        {
            Name = "巴氏鲜牛奶",
            PackingType = PackingType.PlasticBag,
            OrderUnit = OrderUnit.ByOne,
            QuantityPerBox = 20,
            MinimumOrderUnitQuantity = 10,
            UnitPrice = 2m
        };
        public static readonly Product PasteurizedLimitedMilk = new Product
        {
            Name = "巴氏珍品鲜牛奶",
            PackingType = PackingType.PlasticBag,
            OrderUnit = OrderUnit.ByOne,
            QuantityPerBox = 20,
            MinimumOrderUnitQuantity = 10,
            UnitPrice = 2.5m
        };
        public static readonly Product Yogurt = new Product
        {
            Name = "酸牛奶",
            PackingType = PackingType.PlasticBag,
            OrderUnit = OrderUnit.ByOne,
            QuantityPerBox = 20,
            MinimumOrderUnitQuantity = 10,
            UnitPrice = 2m
        };
        public static readonly Product LimitedYogurt = new Product
        {
            Name = "珍品酸牛奶",
            PackingType = PackingType.PlasticBag,
            OrderUnit = OrderUnit.ByBox,
            QuantityPerBox = 20,
            MinimumOrderUnitQuantity = 1,
            UnitPrice = 50m
        };
        public static readonly Product BeijingYogurt = new Product
        {
            Name = "老北京王府酸牛奶",
            PackingType = PackingType.PlasticCup,
            OrderUnit = OrderUnit.ByBox,
            QuantityPerBox = 30,
            MinimumOrderUnitQuantity = 1,
            UnitPrice = 90m,
        };
        public static readonly Product SweetMilk = new Product
        {
            Name = "甜牛奶",
            PackingType = PackingType.PlasticBottle,
            OrderUnit = OrderUnit.ByBox,
            QuantityPerBox = 15,
            MinimumOrderUnitQuantity = 1,
            UnitPrice = 75m,
        };
        public static readonly Product BananaMilk = new Product
        {
            Name = "香蕉牛奶",
            PackingType = PackingType.PlasticBottle,
            OrderUnit = OrderUnit.ByBox,
            QuantityPerBox = 15,
            MinimumOrderUnitQuantity = 2,
            UnitPrice = 72m,
        };
        public static readonly Product FlavoredYogurt = new Product
        {
            Name = "风味酸乳",
            PackingType = PackingType.GlassBottle,
            OrderUnit = OrderUnit.ByOne,
            QuantityPerBox = 15,
            MinimumOrderUnitQuantity = 1,
            UnitPrice = 5m,
        };
        public static readonly Product XylitolYogurt = new Product
        {
            Name = "木糖醇酸奶",
            PackingType = PackingType.GlassBottle,
            OrderUnit = OrderUnit.ByOne,
            QuantityPerBox = 15,
            MinimumOrderUnitQuantity = 1,
            UnitPrice = 6m,
        };
        public static Product LactobacillusYogurt = new Product
        {
            Name = "乳酸菌酸奶",
            PackingType = PackingType.GlassBottle,
            OrderUnit = OrderUnit.ByBox,
            QuantityPerBox = 10,
            MinimumOrderUnitQuantity = 2,
            UnitPrice = 75m,
        };
    }
}
