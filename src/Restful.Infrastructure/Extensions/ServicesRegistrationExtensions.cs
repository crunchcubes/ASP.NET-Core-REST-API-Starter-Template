using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restful.Core.Entities.Milk;
using Restful.Core.Interfaces;
using Restful.Core.Interfaces.Milk;
using Restful.Infrastructure.Database;
using Restful.Infrastructure.Interfaces;
using Restful.Infrastructure.Repositories;
using Restful.Infrastructure.Repositories.Milk;
using Restful.Infrastructure.Repositories.RestDemo;
using Restful.Infrastructure.Resources.Milk.PropertyMappings;
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
            services.AddAutoMapper();

            services.AddTransient<IValidator<CityAddResource>, CityAddOrUpdateResourceValidator<CityAddResource>>();
            services.AddTransient<IValidator<CityUpdateResource>, CityUpdateResourceValidator>();
            services.AddTransient<IValidator<CountryAddResource>, CountryAddResourceValidator>();

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<CountryPropertyMapping>();
            propertyMappingContainer.Register<ProductPropertyMapping>();

            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
            services.AddTransient<ITypeHelperService, TypeHelperService>();

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IEnhancedRepository<>), typeof(EfEnhancedRepository<>));

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IOrderValidator, OrderValidator>();
            services.AddTransient<IDeliveryValidator, DeliveryValidator>();
        }

    }
}
