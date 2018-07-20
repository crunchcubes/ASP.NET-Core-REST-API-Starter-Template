using Restful.Core.Entities.Milk;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class OrdersExceedBoxAreaDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] 
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 101)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 50),
                    (product: Products.SweetMilk, 15)
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 10),
                    (product: Products.SweetMilk, 20),
                    (product: Products.XylitolYogurt, 25),
                }
            };
            yield return new object[]
            {
                new List<(Product product, int quantity)>
                {
                    (product: Products.BeijingYogurt, 100),
                    (product: Products.SweetMilk, 1)
                }
            };
        }
    }
}
