using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using Restful.Core.Entities.Milk;
using Restful.Infrastructure.Extensions;
using Xunit;

namespace Restful.UnitTests.Infrastructure.Extensions
{
    public class EnumerableExtensionShould
    {
        private readonly Mock<IEnumerable<Product>> _mockProducts;

        public EnumerableExtensionShould()
        {
            _mockProducts = new Mock<IEnumerable<Product>>
            {
                DefaultValue = DefaultValue.Mock
            };
            _mockProducts.SetupAllProperties();
        }

        [Fact]
        public void ThrowExceptionWhenSourceNull()
        {
            IEnumerable<Product> source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDynamicIEnumerable());
        }

        [Fact]
        public void HasAllPropertiesWhenFieldsNull()
        {
            var items = CreateItems<Product>(10);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);

            var expandos = _mockProducts.Object.ToDynamicIEnumerable().ToList();
            var propertyInfos = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Assert.NotEmpty(expandos);
            Assert.Equal(10, expandos.Count);

            var expando1 = expandos.First();
            foreach (var propertyInfo in propertyInfos)
            {
                Assert.True(((IDictionary<string, object>)expando1).Keys.Contains(propertyInfo.Name));
            }
        }

        [Theory]
        [InlineData("Id, Name, PackingType")]
        [InlineData("id,Name,PackingType")]
        [InlineData("id, name, PACKINGTYPE")]
        [InlineData("ID,NAME,PACKINGTYPE")]
        [InlineData("id, name, packingType")]
        [InlineData("Id,name,packingType")]
        public void TrueWhenFieldsValidCaseSensitive(string fields)
        {
            var items = CreateItems<Product>(10);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);

            var expandos = _mockProducts.Object.ToDynamicIEnumerable(fields).ToList();

            var expando1 = expandos.First();
            Assert.True(((IDictionary<string, object>)expando1).Keys.Contains("Id"));
            Assert.True(((IDictionary<string, object>)expando1).Keys.Contains("Name"));
            Assert.True(((IDictionary<string, object>)expando1).Keys.Contains("PackingType"));
        }

        [Theory]
        [InlineData("Id, Name, PackingType", "QuantityPerBox")]
        [InlineData("id,Name,PackingType", "OrderUnit")]
        [InlineData("id, name, PACKINGTYPE", "MinimumOrderUnitQuantity")]
        [InlineData("ID,NAME,PACKINGTYPE", "UnitPrice")]
        [InlineData("id, name, packingType", "UnitPrice")]
        [InlineData("Id,name,packingType", "UnitPrice")]
        public void FailWhenPropertyNotInFields(string fields, string property)
        {
            var items = CreateItems<Product>(10);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);

            var expandos = _mockProducts.Object.ToDynamicIEnumerable(fields).ToList();

            var expando1 = expandos.First();
            Assert.False(((IDictionary<string, object>)expando1).Keys.Contains(property));
        }

        [Theory]
        [InlineData("Id, Name, Packin_gType")]
        [InlineData("inimumOrderUnitQuantity, PackingType")]
        [InlineData("id1,Name,PackingType")]
        [InlineData("id, name_, PACKINGTYPE")]
        [InlineData("ID,NAME,_PACKINGTYPE")]
        [InlineData("id, name'packingType")]
        [InlineData("Idname,packingType")]
        [InlineData("UnitPrice,name`MinimumOrderUnitQuantity,")]
        [InlineData(",UnitPrice,name;MinimumOrderUnitQuantity,")]
        public void ThrowExceptionWhenInvalidFields(string fields)
        {
            var items = CreateItems<Product>(10);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);

            Assert.Throws<Exception>(() => _mockProducts.Object.ToDynamicIEnumerable(fields));
        }

        private IEnumerator<T> CreateItems<T>(int count) where T : class
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Mock<T>().Object;
            }
        }
    }
}
