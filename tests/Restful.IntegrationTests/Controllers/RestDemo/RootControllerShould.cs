using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Restful.Infrastructure.Resources.Hateoas;
using Xunit;

namespace Restful.IntegrationTests.Controllers.RestDemo
{
    [Collection("my collection")]
    public class RootControllerShould
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
            var postRequest = new HttpRequestMessage(HttpMethod.Get, "/api");
            postRequest.Headers.Add("Accept", "application/vnd.solenovex.hateoas+json");
            var response = await _fixture.Client.SendAsync(postRequest);

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
