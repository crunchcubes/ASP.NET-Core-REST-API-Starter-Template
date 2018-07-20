using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace Restful.UnitTests.Core.Entities.Milk.Data
{
    class ProductByBoxWithInvalidMinQuantityDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { 15, Products.LimitedYogurt };
            yield return new object[] { 17, Products.BeijingYogurt };
            yield return new object[] { 1, Products.SweetMilk };
            yield return new object[] { 15, Products.BananaMilk };
        }
    }
}
