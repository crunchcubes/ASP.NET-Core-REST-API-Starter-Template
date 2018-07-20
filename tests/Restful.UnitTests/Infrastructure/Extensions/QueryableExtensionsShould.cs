using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using Restful.Core.Entities.Milk;
using Restful.Infrastructure.Extensions;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Resources.Milk.PropertyMappings;
using Restful.Infrastructure.Services;
using Xunit;

namespace Restful.UnitTests.Infrastructure.Extensions
{
    public class QueryableExtensionsShould
    {
        private readonly Mock<IQueryable<Product>> _mockProducts;
        private readonly Mock<IPropertyMapping> _mockPropertyMapping;

        public QueryableExtensionsShould()
        {
            _mockProducts = new Mock<IQueryable<Product>>
            {
                DefaultValue = DefaultValue.Mock
            };
            _mockProducts.SetupAllProperties();
            _mockPropertyMapping = new Mock<IPropertyMapping>
            {
                DefaultValue = DefaultValue.Mock
            };
            _mockPropertyMapping.SetupAllProperties();
        }

        #region ApplySort

        [Fact]
        public void ThrowExceptionWhenSourceNull()
        {
            IQueryable<Product> source = null;
            Assert.Throws<ArgumentNullException>(() => source.ApplySort(null, null));
        }

        [Fact]
        public void ThrowExceptionWhenPropertyMappingNull()
        {
            Assert.Throws<ArgumentNullException>(() => _mockProducts.Object.ApplySort(null, null));
        }

        [Fact]
        public void ThrowExceptionWhenPropertyMappingsDictonaryNull()
        {
            _mockPropertyMapping.Setup(x => x.MappingDictionary).Returns(value: null);
            Assert.Throws<ArgumentNullException>(() => _mockProducts.Object.ApplySort(null, _mockPropertyMapping.Object));
        }

        [Theory]
        [InlineData("")]
        [InlineData(",")]
        [InlineData(null)]
        [InlineData(" ")]
        [InlineData("    ")]
        public void ReturnSourceWhenOrderByEmpty(string orderBy)
        {
            var products = CreateProducts().AsQueryable();

            var result = products.ApplySort(orderBy, _mockPropertyMapping.Object);

            Assert.Equal(products.Count(), result.Count());
        }

        [Theory]
        [InlineData("id,name,name123")]
        [InlineData("ID,Name,1")]
        [InlineData("Name2,ID3")]
        [InlineData("MinimumOrderUnitQuantity")]
        public void ThrowExceptionWhenOrderByPropertyNotInPropertyMappingDictonary(string orderBy)
        {
            _mockPropertyMapping.Setup(x => x.MappingDictionary).Returns(new ProductPropertyMapping().MappingDictionary);

            var items = CreateItems<Product>(5);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);

            Assert.Throws<ArgumentException>(() => _mockProducts.Object.ApplySort(orderBy, _mockPropertyMapping.Object));
        }

        [Theory]
        [InlineData("Name", "Name")]
        [InlineData("Id", "Id,")]
        [InlineData("MinimumOrderUnitQuantity", ",MinimumOrderUnitQuantity,")]
        public void ThrowExceptionWhenOrderByPropertyFoundInMappingDictonaryButNull(string propertyName, string orderBy)
        {
            var propertyMapping = new ProductPropertyMapping();
            propertyMapping.MappingDictionary[propertyName] = null;
            _mockPropertyMapping.Setup(x => x.MappingDictionary).Returns(propertyMapping.MappingDictionary);

            var items = CreateItems<Product>(5);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);

            Assert.Throws<ArgumentNullException>(() => _mockProducts.Object.ApplySort(orderBy, _mockPropertyMapping.Object));
        }

        [Theory]
        [InlineData("Id", 3)]
        [InlineData("Name, Id,", 5)]
        [InlineData("QuantityPerBox, Name, Id", 1)]
        [InlineData(",UnitPrice, Name, QuantityPerBox, Id", 3)]
        [InlineData("Id desc", 3)]
        [InlineData("Name, Id desc", 2)]
        [InlineData(",QuantityPerBox desc, Name desc, Id", 1)]
        [InlineData("UnitPrice desc, QuantityPerBox desc, Name, Id,", 3)]
        public void SortedWhenValidParameters(string orderBy, int thirdProductId)
        {
            var mockProductPropertyMapping = new Mock<IPropertyMapping>();
            var dict = CreateMappingDictionary();
            mockProductPropertyMapping.Setup(x => x.MappingDictionary).Returns(dict);

            _mockPropertyMapping.Setup(x => x.MappingDictionary).Returns(mockProductPropertyMapping.Object.MappingDictionary);

            var products = CreateProducts().AsQueryable();

            var result = products.ApplySort(orderBy, _mockPropertyMapping.Object);
            var thirdProduct = result.Skip(2).Take(1).Single();

            Assert.NotNull(thirdProduct);

            Assert.Equal(thirdProductId, thirdProduct.Id);
        }

        private IEnumerator<T> CreateItems<T>(int count) where T : class
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Mock<T>().Object;
            }
        }

        private IEnumerable<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 4,
                    Name = "Name1",
                    QuantityPerBox = 20,
                    UnitPrice = 2m
                },
                new Product
                {
                    Id = 2,
                    Name = "Name1",
                    QuantityPerBox = 10,
                    UnitPrice = 2m
                },
                new Product
                {
                    Id = 5,
                    Name = "Name1",
                    QuantityPerBox = 20,
                    UnitPrice = 5m
                },
                new Product
                {
                    Id = 3,
                    Name = "Name2",
                    QuantityPerBox = 10,
                    UnitPrice = 2.5m
                },new Product
                {
                    Id = 1,
                    Name = "Name2",
                    QuantityPerBox = 20,
                    UnitPrice = 5m
                }
            };
        }

        private Dictionary<string, List<MappedProperty>> CreateMappingDictionary()
        {
            return new Dictionary<string, List<MappedProperty>>
            {
                [nameof(ProductResource.Id)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.Id), Revert = false}
                },
                [nameof(ProductResource.Name)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.Name), Revert = false}
                },
                [nameof(ProductResource.UnitPrice)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.UnitPrice), Revert = false}
                },
                [nameof(ProductResource.QuantityPerBox)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.QuantityPerBox), Revert = true}
                },
                [nameof(ProductResource.PackingType)] = new List<MappedProperty>
                {
                    new MappedProperty{ Name = nameof(Product.PackingType), Revert = true}
                }
            };
        }

        #endregion

        #region ToDynamic

        [Fact]
        public void ThrowExceptionWhenSourceNullForToDynamicQueryable()
        {
            IQueryable<Product> source = null;
            Assert.Throws<ArgumentNullException>(() => source.ToDynamicQueryable(null, null));
        }

        [Fact]
        public void ThrowExceptionWhenMappingDictionaryNull()
        {
            var items = CreateItems<Product>(10);
            _mockProducts.Setup(x => x.GetEnumerator()).Returns(items);
            Assert.Throws<ArgumentNullException>(() => _mockProducts.Object.ToDynamicQueryable(null, null));
        }

        [Fact]
        public void HasAllPropertiesWhenFieldsNull()
        {
            var products = CreateProducts().AsQueryable();
            var dict = CreateMappingDictionary();
            var objs = products.ToDynamicQueryable(null, dict).ToList();
            var propertyInfos = typeof(Product).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Assert.NotEmpty(objs);
            Assert.Equal(products.Count(), objs.Count);

            var obj1 = objs.First();
            var obj1PropertyInfos = obj1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertyNames = obj1PropertyInfos.Select(x => x.Name).ToList();
            foreach (var propertyInfo in propertyInfos)
            {
                Assert.Contains(propertyInfo.Name, propertyNames);
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
            var products = CreateProducts().AsQueryable();
            var dict = CreateMappingDictionary();
            var objs = products.ToDynamicQueryable(fields, dict).ToList();

            var obj1 = objs.First();
            var obj1PropertyInfos = obj1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertyNames = obj1PropertyInfos.Select(x => x.Name).ToList();

            Assert.Contains("Id", propertyNames);
            Assert.Contains("Name", propertyNames);
            Assert.Contains("PackingType", propertyNames);
        }

        [Theory]
        [InlineData("Id,name,packingType", "Id")]
        [InlineData("id, name, packingType", "PackingType")]
        [InlineData("ID,NAME,PACKINGTYPE", "Name")]
        public void ThrowExceptionWhenMappingPropertyNull(string fields, string mappingNullProperty)
        {
            var products = CreateProducts().AsQueryable();
            var dict = CreateMappingDictionary();
            dict[mappingNullProperty] = null;
            Assert.Throws<ArgumentNullException>(() => products.ToDynamicQueryable(fields, dict));
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
            var products = CreateProducts().AsQueryable();
            var dict = CreateMappingDictionary();
            var objs = products.ToDynamicQueryable(fields, dict).ToList();

            var obj1 = objs.First();
            var obj1PropertyInfos = obj1.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propertyNames = obj1PropertyInfos.Select(x => x.Name).ToList();

            Assert.DoesNotContain(property, propertyNames);
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
            var products = CreateProducts().AsQueryable();
            var dict = CreateMappingDictionary();
            Assert.Throws<ArgumentException>(() => products.ToDynamicQueryable(fields, dict));
        }

        #endregion
    }
}
