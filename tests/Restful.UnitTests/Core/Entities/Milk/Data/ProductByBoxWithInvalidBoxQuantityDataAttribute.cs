using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class ProductByBoxWithInvalidBoxQuantityDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 15, Products.LimitedYogurt };
            yield return new object[] { 43, Products.LimitedYogurt };
            yield return new object[] { 32, Products.BeijingYogurt };
            yield return new object[] { 56, Products.BeijingYogurt };
        }
    }
}
