using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class ProductByBoxWithValidQuantityDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 20, Products.LimitedYogurt };
            yield return new object[] { 40, Products.LimitedYogurt };
            yield return new object[] { 30, Products.BeijingYogurt };
            yield return new object[] { 90, Products.BeijingYogurt };
        }
    }
}
