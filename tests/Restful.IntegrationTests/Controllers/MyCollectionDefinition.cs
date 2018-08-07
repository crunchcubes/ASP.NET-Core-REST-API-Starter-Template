using Xunit;

namespace Restful.IntegrationTests.Controllers
{
    [CollectionDefinition("my collection")]
    public class MyCollectionDefinition : ICollectionFixture<TestServerFixture> { }

}
