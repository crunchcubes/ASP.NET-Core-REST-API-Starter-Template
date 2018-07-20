using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Restful.Api.Controllers.Milk;
using Restful.Core.Entities.Milk;
using Restful.Core.Interfaces;
using Restful.Core.Interfaces.Milk;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Services;
using Xunit;

namespace Restful.UnitTests.Api.Controllers.Milk
{
    public class ProductControllerShould
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly Mock<IPropertyMappingContainer> _mockPropertyMappingContainer;
        private readonly Mock<ITypeHelperService> _mockTypeHelperService;

        private readonly ProductController _sut;

        public ProductControllerShould()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper> { DefaultValue = DefaultValue.Mock };
            _mockUrlHelper = new Mock<IUrlHelper>();
            _mockPropertyMappingContainer = new Mock<IPropertyMappingContainer>();
            _mockTypeHelperService = new Mock<ITypeHelperService>();

            _sut = new ProductController(
                _mockUnitOfWork.Object,
                _mockProductRepository.Object,
                _mockMapper.Object,
                _mockUrlHelper.Object,
                _mockPropertyMappingContainer.Object,
                _mockTypeHelperService.Object);
        }

        [Fact]
        public async Task ReturnUnprocessableEntityObjectResultWhenInvalidModelState()
        {
            _sut.ModelState.AddModelError("SomeProperty", "A test error");

            var productAddResource = new ProductAddResource();
            var result = await _sut.CreateProduct(productAddResource);

            Assert.IsType<UnprocessableEntityObjectResult>(result);

            var objectResult = result as ObjectResult;

            Assert.NotNull(objectResult?.Value);

            var modelState = objectResult.Value as dynamic;

            Assert.True(modelState?.ContainsKey("SomeProperty"));
        }

        [Fact]
        public async Task SaveWhenModelStateValid()
        {
            _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(true);

            var productAddResource = new ProductAddResource();
            await _sut.CreateProduct(productAddResource);

            _mockProductRepository.Verify(x => x.AddProduct(It.IsAny<Product>()));

            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ProductSavedWhenModelStateValid()
        {
            var productFromMapper = new Product
            {
                Name = "Name",
                QuantityPerBox = 10
            };
            _mockMapper.Setup(x => x.Map<Product>(It.IsAny<ProductAddResource>())).Returns(productFromMapper);

            var productResourceFromMapper = new ProductResource
            {
                Id = 5,
                Name = "Name",
                QuantityPerBox = 10
            };
            _mockMapper.Setup(x => x.Map<ProductResource>(productFromMapper)).Returns(productResourceFromMapper);

            Product productModelAfterSaved = null;
            _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(true)
                .Callback(() => productModelAfterSaved = new Product
                {
                    Id = 5,
                    Name = productFromMapper.Name,
                    QuantityPerBox = productFromMapper.QuantityPerBox
                });

            var productAddResource = new ProductAddResource();
            var result = await _sut.CreateProduct(productAddResource);
            var createdAtRouteResult = result as CreatedAtRouteResult;

            _mockProductRepository.Verify(x => x.AddProduct(It.IsAny<Product>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);

            Assert.IsType<CreatedAtRouteResult>(result);
            Assert.NotNull(createdAtRouteResult);
            Assert.IsAssignableFrom<IDictionary<string, object>>(createdAtRouteResult.Value);

            var dict = createdAtRouteResult.Value as IDictionary<string, object>;
            Assert.NotNull(dict);
            Assert.Equal(productModelAfterSaved.Id, (int)createdAtRouteResult.RouteValues["id"]);
            Assert.Equal(productModelAfterSaved.Name, (string)dict[nameof(productResourceFromMapper.Name)]);
            Assert.Equal(productModelAfterSaved.QuantityPerBox, (int)dict[nameof(productResourceFromMapper.QuantityPerBox)]);
        }

        [Fact]
        public async Task ReturnBadRequestWhenProductNull()
        {
            var result = await _sut.CreateProduct(null);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task ThrowExceptionWhenSaveFailed()
        {
            _mockUnitOfWork.Setup(x => x.SaveAsync()).ReturnsAsync(false);

            var productAddResource = new ProductAddResource();
            await Assert.ThrowsAsync<Exception>(async () => await _sut.CreateProduct(productAddResource));
        }

    }
}
