using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class ProductByOneWithInvalidQuantityDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 1, Products.PasteurizedMilk };
            yield return new object[] { 9, Products.PasteurizedMilk };
            yield return new object[] { 5, Products.Yogurt };
            yield return new object[] { 6, Products.Yogurt };
        }
    }
}
