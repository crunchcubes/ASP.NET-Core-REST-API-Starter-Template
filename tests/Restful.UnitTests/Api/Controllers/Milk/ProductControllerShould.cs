using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Restful.Api.Controllers.Milk;
using Restful.Core.Interfaces;
using Restful.Core.Interfaces.Milk;
using Restful.Infrastructure.Services;

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
            _mockMapper = new Mock<IMapper>();
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


    }
}
