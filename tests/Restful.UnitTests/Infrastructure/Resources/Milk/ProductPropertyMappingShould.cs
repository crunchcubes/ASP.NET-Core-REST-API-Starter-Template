using Restful.Infrastructure.Resources.Milk.PropertyMappings;
using Xunit;

namespace Restful.UnitTests.Infrastructure.Resources.Milk
{
    public class ProductPropertyMappingShould
    {
        private readonly ProductPropertyMapping _sut;

        public ProductPropertyMappingShould()
        {
            _sut = new ProductPropertyMapping();
        }

        [Fact]
        public void HasIdMapping()
        {
            Assert.NotNull(_sut.MappingDictionary["Id"]);
            Assert.NotNull(_sut.MappingDictionary["id"]);
        }
    }
}
