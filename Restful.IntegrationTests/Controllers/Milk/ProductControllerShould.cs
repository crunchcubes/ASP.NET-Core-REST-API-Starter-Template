using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Restful.Core.Entities.Milk;
using Restful.Infrastructure.Resources.Milk;
using Xunit;

namespace Restful.IntegrationTests.Controllers.Milk
{
    public class ProductControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public ProductControllerShould(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task SuccessWhenValid()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/products");
            postRequest.Headers.Add("Accept", "application/vnd.solenovex.product.display+json");
            postRequest.Content = new StringContent(
                JsonConvert.SerializeObject(new ProductAddResource
                {
                    Name = "Milk",
                    OrderUnit = OrderUnit.ByBox,
                    PackingType = PackingType.GlassBottle,
                    MinimumOrderQuantity = 1,
                    QuantityPerBox = 10,
                    UnitPrice = 5m
                }));
            postRequest.Content.Headers.ContentType = 
                new MediaTypeHeaderValue("application/vnd.solenovex.product.create+json");

            var response = await _fixture.Client.SendAsync(postRequest);

            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(content);
            var product = JsonConvert.DeserializeObject<ProductResource>(content);
            Assert.NotNull(product);
            Assert.True(product.Id > 0);
        }

        [Fact]
        public async Task ReturnUnprocessableEntityObjectResultWhenInvalidProduct()
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/api/products");
            postRequest.Headers.Add("Accept", "application/vnd.solenovex.product.display+json");
            postRequest.Content = new StringContent(
                JsonConvert.SerializeObject(new ProductAddResource
                {
                    Name = "A very long name...........................",
                    OrderUnit = OrderUnit.ByBox,
                    PackingType = PackingType.GlassBottle,
                    MinimumOrderQuantity = 1,
                    QuantityPerBox = 10,
                    UnitPrice = 5m
                }));
            postRequest.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/vnd.solenovex.product.create+json");

            var response = await _fixture.Client.SendAsync(postRequest);

            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(content);

            var modelState = JsonConvert.DeserializeObject<dynamic>(content);
            Assert.NotNull(modelState);
            Assert.NotNull(modelState.name);
        }

    }
}
