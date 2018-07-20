using System;
using System.Linq;
using Restful.Core.Entities;
using Restful.Core.Entities.Milk;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Resources.Milk.PropertyMappings;
using Restful.Infrastructure.Resources.RestDemo;
using Restful.Infrastructure.Resources.RestDemo.PropertyMappings;
using Restful.Infrastructure.Services;
using Xunit;

namespace Restful.UnitTests.Infrastructure.Services
{
    public class PropertyMappingContainerShould
    {
        private readonly PropertyMappingContainer _propertyMappingContainer;

        public PropertyMappingContainerShould()
        {
            _propertyMappingContainer = new PropertyMappingContainer();
        }

        [Fact]
        public void SuccessfullyRegister()
        {
            _propertyMappingContainer.Register<ProductPropertyMapping>();

            Assert.IsType<ProductPropertyMapping>(_propertyMappingContainer.PropertyMappings.Single());
        }

        [Fact]
        public void SuccessfullyRegisterSameTypeForMultipleTimes()
        {
            _propertyMappingContainer.Register<ProductPropertyMapping>();
            _propertyMappingContainer.Register<ProductPropertyMapping>();
            _propertyMappingContainer.Register<ProductPropertyMapping>();
            _propertyMappingContainer.Register<ProductPropertyMapping>();

            Assert.IsType<ProductPropertyMapping>(_propertyMappingContainer.PropertyMappings.Single());
        }

        [Fact]
        public void SuccessfullyResolveRegisteredPropertyMappings()
        {
            _propertyMappingContainer.Register<ProductPropertyMapping>();
            _propertyMappingContainer.Register<CountryPropertyMapping>();

            var resolvedProductMapping = _propertyMappingContainer.Resolve<ProductResource, Product>();
            Assert.IsType<ProductPropertyMapping>(resolvedProductMapping);

            var resolvedCountryMapping = _propertyMappingContainer.Resolve<CountryResource, Country>();
            Assert.IsType<CountryPropertyMapping>(resolvedCountryMapping);
        }

        [Fact]
        public void ThrowExceptionWhenNotResolve()
        {
            _propertyMappingContainer.Register<ProductPropertyMapping>();

            Assert.Throws<Exception>(() => _propertyMappingContainer.Resolve<CountryResource, Country>());
        }

        [Theory]
        [InlineData("", true)]
        [InlineData(" ", true)]
        [InlineData("  ", true)]
        [InlineData("    ", true)]
        [InlineData(",  ", true)]
        [InlineData(",  , ", true)]
        [InlineData(",,,", true)]
        [InlineData("Id,Name,UnitPrice", true)]
        [InlineData("Id,name,UNITPRICE", true)]
        [InlineData("id,Name,unitprice,", true)]
        [InlineData(",id,", true)]
        [InlineData("Id,Name.UnitPrice", false)]
        [InlineData("Ida,name,UNITPRICE", false)]
        [InlineData("id_Name_unitprice,", false)]
        [InlineData(",id,.", false)]
        public void SuccessfullyValidateMappingsExistForProductPropertyMapping(string fields, bool expect)
        {
            _propertyMappingContainer.Register<ProductPropertyMapping>();
            _propertyMappingContainer.Register<CountryPropertyMapping>();

            Assert.Equal(expect, _propertyMappingContainer.ValidateMappingExistsFor<ProductResource, Product>(fields));
        }

    }
}
