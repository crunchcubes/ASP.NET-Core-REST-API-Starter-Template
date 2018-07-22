using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Restful.Api;
using Restful.Infrastructure.Resources.Hateoas;
using Xunit;

namespace Restful.IntegrationTests.Controllers.RestDemo
{
    public class RootControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public RootControllerShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task NoContentWhenOtherMediaType()
        {
            var response = await _fixture.Client.GetAsync("/api");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Empty(content);
        }

        [Fact]
        public async Task ReturnLinkResourceListWhenSpecificMediaType()
        {
            _fixture.Client.DefaultRequestHeaders.Add("Accept", "application/vnd.solenovex.hateoas+json");
            var response = await _fixture.Client.GetAsync("/api");

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonStr = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<LinkResource>>(jsonStr);
            Assert.True(list.Count > 0);

            var selfLink = list.Single(x => x.Rel == "self");
            Assert.NotNull(selfLink);
        }
    }
}
