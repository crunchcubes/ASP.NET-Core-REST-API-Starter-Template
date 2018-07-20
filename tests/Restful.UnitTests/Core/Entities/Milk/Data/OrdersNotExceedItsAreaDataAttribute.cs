using Restful.Core.Entities.Milk;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class OrdersNotExceedItsAreaDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] 
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 1),
                    (product: Products.PasteurizedMilk, 1)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 100),
                    (product: Products.PasteurizedMilk, 10)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 50),
                    (product: Products.SweetMilk, 10),
                    (product: Products.PasteurizedMilk, 10)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 50),
                    (product: Products.SweetMilk, 10),
                    (product: Products.PasteurizedMilk, 5),
                    (product: Products.PasteurizedLimitedMilk, 5)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 50),
                    (product: Products.SweetMilk, 10)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.PasteurizedMilk, 5),
                    (product: Products.PasteurizedLimitedMilk, 5)
                }
            };
        }
    }
}
