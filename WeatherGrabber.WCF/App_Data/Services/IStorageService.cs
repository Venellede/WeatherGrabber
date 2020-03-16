using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeatherGrabber.WCF.Model;

namespace WeatherGrabber.WCF.Services
{
    public interface IStorageService
    {
        Task<IEnumerable<string>> GetCities(CancellationToken token = default);
        Task<IEnumerable<DateTime>> GetDatesForCity(string cityName, CancellationToken token = default);
        Task<Weather> GetWeather(string cityName, DateTime dateTime, CancellationToken token = default);
    }
}
