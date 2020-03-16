using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using WeatherGrabber.WCF.Model;
using WeatherGrabber.WCF.Services;

namespace WeatherGrabber.WCF
{
    public class WeatherGrabberService : IWeatherGrabberService
    {
        IStorageService _storage;
        public WeatherGrabberService()
        {
            _storage = new MySqlStorageSerivce(ConfigurationManager.AppSettings["server"], ConfigurationManager.AppSettings["database"], ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
        }
        public Task<IEnumerable<string>> GetCities()
        {
            return _storage.GetCities();
        }

        public Task<Weather> GetWeather(string cityName, DateTime dateTime)
        {
            return _storage.GetWeather(cityName, dateTime);
        }
    }
}
