using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class ProductByOneWithValidQuantityDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 11, Products.PasteurizedMilk };
            yield return new object[] { 29, Products.PasteurizedMilk };
            yield return new object[] { 35, Products.Yogurt };
            yield return new object[] { 16, Products.Yogurt };
        }
    }
}
