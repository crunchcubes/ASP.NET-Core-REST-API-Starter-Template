using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Restful.Core.Entities;
using Restful.Core.Entities.Milk;
using Restful.Core.Entities.RestDemo;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Resources.RestDemo;

namespace Restful.Infrastructure.Configuration
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryResource>();
            CreateMap<CountryResource, Country>();
            CreateMap<CountryAddResource, Country>();
            CreateMap<CountryUpdateResource, Country>()
                .ForMember(c => c.Cities, opt => opt.Ignore())
                .AfterMap((countryUpdateResource, country) =>
                {
                    // Remove
                    var countryUpdateCityIds = countryUpdateResource.Cities.Select(x => x.Id).ToList();
                    var removedCities = country.Cities.Where(c => !countryUpdateCityIds.Contains(c.Id)).ToList();
                    foreach (var city in removedCities)
                    {
                        country.Cities.Remove(city);
                    }
                    // Add
                    var addedCityResources = countryUpdateResource.Cities.Where(x => x.Id == 0);
                    var addedCities = Mapper.Map<IEnumerable<City>>(addedCityResources);
                    foreach (var city in addedCities)
                    {
                        country.Cities.Add(city);
                    }
                    // Update or Unchanged
                    var maybeUpdateCities = country.Cities.Where(x => x.Id != 0).ToList();
                    foreach (var city in maybeUpdateCities)
                    {
                        var cityResource = countryUpdateResource.Cities.Single(x => x.Id == city.Id);
                        Mapper.Map(cityResource, city);
                    }
                });
            CreateMap<CountryAddWithContinentResource, Country>();

            CreateMap<City, CityResource>();
            CreateMap<CityResource, City>();
            CreateMap<CityAddResource, City>();
            CreateMap<CityUpdateResource, City>();
            CreateMap<City, CityUpdateResource>();

            // Milk
            CreateMap<Product, ProductResource>();
            CreateMap<Product, ProductUpdateResource>();
            CreateMap<ProductResource, Product>();
            CreateMap<ProductAddResource, Product>();
            CreateMap<ProductUpdateResource, Product>();
        }
    }
}
