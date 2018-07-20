using System.Collections.Generic;
using System.Threading.Tasks;
using Restful.Core.Entities;
using Restful.Core.Entities.RestDemo;

namespace Restful.Core.Interfaces
{
    public interface ICityRepository
    {
        Task<List<City>> GetCitiesForCountryAsync(int countryId);
        Task<City> GetCityForCountryAsync(int countryId, int cityId);
        void AddCityForCountry(int countryId, City city);
        void DeleteCity(City city);
        void UpdateCityForCountry(City city);
    }
}
