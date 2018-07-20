using System.Collections.Generic;
using System.Linq;
using Restful.Core.Entities.Milk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class OrderTestData
    {
        public static IEnumerable<object[]> MixOfValidAndInvalidOrders
        {
            get
            {
                var data1 = new ProductByBoxWithInvalidBoxQuantityDataAttribute().GetData(null)
                    .Concat(new ProductByOneWithInvalidQuantityDataAttribute().GetData(null))
                    .Concat(new ProductByBoxWithInvalidMinQuantityDataAttribute().GetData(null))
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));

                var data2 = new ProductByBoxWithInvalidBoxQuantityDataAttribute().GetData(null)
                    .Concat(new ProductByOneWithInvalidQuantityDataAttribute().GetData(null))
                    .Concat(new ProductByOneWithValidQuantityDataAttribute().GetData(null))
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));

                var data3 = new ProductByBoxWithInvalidBoxQuantityDataAttribute().GetData(null)
                    .Concat(new ProductByBoxWithValidQuantityDataAttribute().GetData(null))
                    .Concat(new ProductByBoxWithInvalidMinQuantityDataAttribute().GetData(null))
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));

                var data4 = new ProductByBoxWithInvalidMinQuantityDataAttribute().GetData(null)
                    .Concat(new ProductByBoxWithValidQuantityDataAttribute().GetData(null))
                    .Concat(new ProductByOneWithValidQuantityDataAttribute().GetData(null))
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));

                return new List<object[]> { new object[] { data1 }, new object[] { data2 }, new object[] { data3 }, new object[] { data4 } };
            }
        }

        public static IEnumerable<object[]> ValidOrders
        {
            get
            {
                var data1 = new ProductByBoxWithValidQuantityDataAttribute().GetData(null)
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));


                var data2 = new ProductByOneWithValidQuantityDataAttribute().GetData(null)
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));


                var data3 = new ProductByOneWithValidQuantityDataAttribute().GetData(null)
                    .Concat(new ProductByBoxWithValidQuantityDataAttribute().GetData(null))
                    .Select(x => (product: (Product)x[1], quantity: (int)x[0]));


                return new List<object[]> { new object[] { data1 }, new object[] { data2 }, new object[] { data3 } };
            }
        }
    }
}
