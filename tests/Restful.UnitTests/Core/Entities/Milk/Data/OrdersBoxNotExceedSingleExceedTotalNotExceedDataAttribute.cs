using Restful.Core.Entities.Milk;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class OrdersBoxNotExceedSingleExceedTotalNotExceedDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] 
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 1),
                    (product: Products.PasteurizedMilk, 50)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 90),
                    (product: Products.PasteurizedMilk, 15)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 40),
                    (product: Products.SweetMilk, 10),
                    (product: Products.PasteurizedMilk, 15)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.PasteurizedMilk, 50),
                    (product: Products.PasteurizedLimitedMilk, 10)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 20),
                    (product: Products.PasteurizedMilk, 50)
                }
            };
        }
    }
}
