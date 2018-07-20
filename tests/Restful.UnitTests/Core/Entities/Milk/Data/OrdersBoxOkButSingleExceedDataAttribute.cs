using Restful.Core.Entities.Milk;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class OrdersBoxOkButSingleExceedDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] 
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 100),
                    (product: Products.PasteurizedMilk, 11)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 90),
                    (product: Products.PasteurizedMilk, 16)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 21),
                    (product: Products.SweetMilk, 10),
                    (product: Products.PasteurizedMilk, 25)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 30),
                    (product: Products.FlavoredYogurt, 10),
                    (product: Products.PasteurizedMilk, 20),
                    (product: Products.PasteurizedLimitedMilk, 6)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.PasteurizedMilk, 25),
                    (product: Products.PasteurizedLimitedMilk, 25),
                    (product: Products.PasteurizedLimitedMilk, 21)
                }
            };
        }
    }
}
