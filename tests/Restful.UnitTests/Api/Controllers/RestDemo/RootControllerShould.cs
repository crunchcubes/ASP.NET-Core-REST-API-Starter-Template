using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Restful.Api.Controllers;
using Restful.Infrastructure.Resources.Hateoas;
using Xunit;

namespace Restful.UnitTests.Api.Controllers.RestDemo
{
    public class RootControllerShould
    {
        private readonly Mock<IUrlHelper> _urlHelper = new Mock<IUrlHelper>();

        private readonly RootController _sut;

        public RootControllerShould()
        {
            _sut = new RootController(_urlHelper.Object);
        }

        [Fact]
        public void ReturnNoContentWhenNotHateoasMediaType()
        {
            var result = _sut.GetRoot("application/json");

            Assert.IsType<NoContentResult>(result);
        }
        
        [Fact]
        public void ReturnOkWhenCompanyHateoasMediaType()
        {
            var result = _sut.GetRoot("application/vnd.solenovex.hateoas+json");

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ReturnLinksInOkWhenCompanyHateoasMediaType()
        {
            var result = _sut.GetRoot("application/vnd.solenovex.hateoas+json");

            var objectResult = result as ObjectResult;

            Assert.IsType<List<LinkResource>>(objectResult?.Value);
        }
    }
}
