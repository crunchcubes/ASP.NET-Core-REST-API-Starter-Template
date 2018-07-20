using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Restful.Api.Helpers;
using Restful.Core.Entities.Milk;
using Restful.Core.Helpers;
using Restful.Core.Helpers.Milk;
using Restful.Core.Interfaces;
using Restful.Core.Interfaces.Milk;
using Restful.Infrastructure.Extensions;
using Restful.Infrastructure.Resources.Hateoas;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Services;

namespace Restful.Api.Controllers.Milk
{
    [Route("api/products")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingContainer _propertyMappingContainer;
        private readonly ITypeHelperService _typeHelperService;
        
        public ProductController(
            IUnitOfWork unitOfWork,
            IProductRepository productRepository,
            IMapper mapper,
            IUrlHelper urlHelper,
            IPropertyMappingContainer propertyMappingContainer,
            ITypeHelperService typeHelperService)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _mapper = mapper;
            _urlHelper = urlHelper;
            _propertyMappingContainer = propertyMappingContainer;
            _typeHelperService = typeHelperService;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> GetProducts(ProductResourceParameters productResourceParameters,
            [FromHeader(Name = "Accept")] string mediaType)
        {
            if (!_propertyMappingContainer.ValidateMappingExistsFor<ProductResource, Product>(productResourceParameters.OrderBy))
            {
                return BadRequest("Can't find the fields for sorting.");
            }

            if (!_typeHelperService.TypeHasProperties<ProductResource>(productResourceParameters.Fields))
            {
                return BadRequest("Can't find the fields on Resource.");
            }

            var pagedList = await _productRepository.GetProductsAsync(productResourceParameters);
            var productResources = _mapper.Map<List<ProductResource>>(pagedList);

            if (mediaType == "application/vnd.solenovex.hateoas+json")
            {
                var meta = new
                {
                    pagedList.TotalItemsCount,
                    pagedList.PaginationBase.PageSize,
                    pagedList.PaginationBase.PageIndex,
                    pagedList.PageCount
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));

                var links = CreateLinksForProducts(productResourceParameters, pagedList.HasPrevious, pagedList.HasNext);
                var shapedResources = productResources.ToDynamicIEnumerable(productResourceParameters.Fields);
                var shapedResourcesWithLinks = shapedResources.Select(product =>
                {
                    var productDict = product as IDictionary<string, object>;
                    var productLinks = CreateLinksForProduct((int)productDict["Id"], productResourceParameters.Fields);
                    productDict.Add("links", productLinks);
                    return productDict;
                });
                var linkedProducts = new
                {
                    value = shapedResourcesWithLinks,
                    links
                };

                return Ok(linkedProducts);
            }
            else
            {
                var previousPageLink = pagedList.HasPrevious ?
                    CreateProductUri(productResourceParameters,
                        PaginationResourceUriType.PreviousPage) : null;

                var nextPageLink = pagedList.HasNext ?
                    CreateProductUri(productResourceParameters,
                        PaginationResourceUriType.NextPage) : null;

                var meta = new
                {
                    pagedList.TotalItemsCount,
                    pagedList.PaginationBase.PageSize,
                    pagedList.PaginationBase.PageIndex,
                    pagedList.PageCount,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(meta, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));

                return Ok(productResources.ToDynamicIEnumerable(productResourceParameters.Fields));
            }
        }
        
        [DisableCors]
        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id, string fields = null)
        {
            if (!_typeHelperService.TypeHasProperties<ProductResource>(fields))
            {
                return BadRequest("Can't find the fields on Resource.");
            }
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productResource = _mapper.Map<ProductResource>(product);

            var links = CreateLinksForProduct(id, fields);
            var result = productResource.ToDynamic(fields) as IDictionary<string, object>;
            result.Add("links", links);

            return Ok(result);
        }

        [HttpPost(Name = "CreateProduct")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/vnd.solenovex.product.create+json" })]
        [RequestHeaderMatchingMediaType("Accept", new[] { "application/vnd.solenovex.product.display+json" })]
        public async Task<IActionResult> CreateProduct([FromBody] ProductAddResource product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            var productModel = _mapper.Map<Product>(product);
            _productRepository.AddProduct(productModel);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception("Error occurred when adding");
            }

            var productResource = Mapper.Map<ProductResource>(productModel);

            var links = CreateLinksForProduct(productModel.Id);
            var linkedProductResource = productResource.ToDynamic() as IDictionary<string, object>;
            linkedProductResource.Add("links", links);

            return CreatedAtRoute("GetProduct", new { id = linkedProductResource["Id"] }, linkedProductResource);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> BlockCreatingProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return StatusCode(StatusCodes.Status409Conflict);
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.DeleteProduct(product);

            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Deleting product {id} failed when saving.");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        [RequestHeaderMatchingMediaType("Content-Type", new[] { "application/vnd.solenovex.product.update+json" })]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateResource productUpdate)
        {
            if (productUpdate == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _mapper.Map(productUpdate, product);
            if (!await _unitOfWork.SaveAsync())
            {
                throw new Exception($"Updating product {id} failed when saving.");
            }
            return NoContent();
        }

        private string CreateProductUri(ProductResourceParameters parameters, PaginationResourceUriType uriType)
        {
            switch (uriType)
            {
                case PaginationResourceUriType.PreviousPage:
                    var previousParameters = new
                    {
                        pageIndex = parameters.PageIndex - 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        name = parameters.Name
                    };
                    return _urlHelper.Link("GetProducts", previousParameters);
                case PaginationResourceUriType.NextPage:
                    var nextParameters = new
                    {
                        pageIndex = parameters.PageIndex + 1,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        name = parameters.Name
                    };
                    return _urlHelper.Link("GetProducts", nextParameters);
                default:
                    var currentParameters = new
                    {
                        pageIndex = parameters.PageIndex,
                        pageSize = parameters.PageSize,
                        orderBy = parameters.OrderBy,
                        fields = parameters.Fields,
                        name = parameters.Name
                    };
                    return _urlHelper.Link("GetProducts", currentParameters);
            }
        }

        private IEnumerable<LinkResource> CreateLinksForProduct(int id, string fields = null)
        {
            var links = new List<LinkResource>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                links.Add(
                    new LinkResource(
                        _urlHelper.Link("GetProduct", new { id }), "self", "GET"));
            }
            else
            {
                links.Add(
                    new LinkResource(
                        _urlHelper.Link("GetProduct", new { id, fields }), "self", "GET"));
            }

            links.Add(
                new LinkResource(
                    _urlHelper.Link("DeleteProduct", new { id }), "delete_product", "DELETE"));

            return links;
        }

        private IEnumerable<LinkResource> CreateLinksForProducts(ProductResourceParameters productResourceParameters,
            bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>
            {
                new LinkResource(
                    CreateProductUri(productResourceParameters, PaginationResourceUriType.CurrentPage),
                    "self", "GET")
            };

            if (hasPrevious)
            {
                links.Add(
                    new LinkResource(
                        CreateProductUri(productResourceParameters, PaginationResourceUriType.PreviousPage),
                        "previous_page", "GET"));
            }

            if (hasNext)
            {
                links.Add(
                    new LinkResource(
                        CreateProductUri(productResourceParameters, PaginationResourceUriType.NextPage),
                        "next_page", "GET"));
            }

            return links;
        }
    }
}
