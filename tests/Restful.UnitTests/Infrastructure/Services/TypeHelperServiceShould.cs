using Restful.Core.Entities;
using Restful.Core.Entities.Milk;
using Restful.Infrastructure.Services;
using Xunit;

namespace Restful.UnitTests.Infrastructure.Services
{
    public class TypeHelperServiceShould
    {
        private readonly TypeHelperService _typeHelperService;

        public TypeHelperServiceShould()
        {
            _typeHelperService = new TypeHelperService();
        }

        [Fact]
        public void HasIdWhenEntity()
        {
            var result = _typeHelperService.TypeHasProperties<Entity>("Id");
            Assert.True(result);

            var result1 = _typeHelperService.TypeHasProperties<Entity>("id");
            Assert.True(result1);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("    ")]
        [InlineData(",")]
        [InlineData(" , , ")]
        [InlineData(",,,,,, ,,,, ,,, ,,")]
        public void BeTrueIfEmptyishFieldsWhenProduct(string fields)
        {
            var result = _typeHelperService.TypeHasProperties<Product>(fields);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Id, Name, PackingType")]
        [InlineData("MinimumOrderUnitQuantity, PackingType")]
        [InlineData("id,Name,PackingType")]
        [InlineData("id, name, PACKINGTYPE")]
        [InlineData("ID,NAME,PACKINGTYPE")]
        [InlineData("id, name, packingType")]
        [InlineData("Id,name,packingType")]
        [InlineData("UnitPrice,name,MinimumOrderUnitQuantity,")]
        [InlineData(",UnitPrice,name,MinimumOrderUnitQuantity,")]
        public void HasValidFieldsWhenProduct(string fields)
        {
            var result = _typeHelperService.TypeHasProperties<Product>(fields);
            Assert.True(result);
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
        public void DoesNotHaveInvalidFieldsWhenProduct(string fields)
        {
            var result = _typeHelperService.TypeHasProperties<Product>(fields);
            Assert.False(result);
        }

    }
}
