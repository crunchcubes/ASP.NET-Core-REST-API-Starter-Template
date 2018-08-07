using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Restful.Core.Entities.Milk;
using Restful.Core.Interfaces;
using Restful.Core.Interfaces.Milk;
using Restful.Infrastructure.Database;
using Restful.Infrastructure.Interfaces;
using Restful.Infrastructure.Repositories;
using Restful.Infrastructure.Repositories.Milk;
using Restful.Infrastructure.Repositories.RestDemo;
using Restful.Infrastructure.Resources.Milk;
using Restful.Infrastructure.Resources.Milk.PropertyMappings;
using Restful.Infrastructure.Resources.Milk.Validators;
using Restful.Infrastructure.Resources.RestDemo;
using Restful.Infrastructure.Resources.RestDemo.PropertyMappings;
using Restful.Infrastructure.Resources.RestDemo.Validators;
using Restful.Infrastructure.Services;

namespace Restful.Infrastructure.Extensions
{
    public static class ServicesRegistrationExtensions
    {
        public static void AddMyServices(this IServiceCollection services)
        {
            Mapper.Reset();
            services.AddAutoMapper();

            services.TryAddTransient<IValidator<CityAddResource>, CityAddOrUpdateResourceValidator<CityAddResource>>();
            services.TryAddTransient<IValidator<CityUpdateResource>, CityUpdateResourceValidator>();
            services.TryAddTransient<IValidator<CountryAddResource>, CountryAddResourceValidator>();

            services.TryAddTransient<IValidator<ProductAddResource>, ProductAddOrUpdateResourceValidator<ProductAddResource>>();
            services.TryAddTransient<IValidator<ProductUpdateResource>, ProductAddOrUpdateResourceValidator<ProductUpdateResource>>();

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<CountryPropertyMapping>();
            propertyMappingContainer.Register<ProductPropertyMapping>();

            services.TryAddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
            services.TryAddTransient<ITypeHelperService, TypeHelperService>();

            services.TryAddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.TryAddScoped(typeof(IEnhancedRepository<>), typeof(EfEnhancedRepository<>));

            services.TryAddScoped<ICountryRepository, CountryRepository>();
            services.TryAddScoped<ICityRepository, CityRepository>();

            services.TryAddScoped<IProductRepository, ProductRepository>();

            services.TryAddScoped<IUnitOfWork, UnitOfWork>();

            services.TryAddTransient<IOrderValidator, OrderValidator>();
            services.TryAddTransient<IDeliveryValidator, DeliveryValidator>();
        }

    }
}
