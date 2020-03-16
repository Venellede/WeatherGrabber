using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WeatherGrabber.Service.Model;
using WeatherGrabber.Service.Services;

namespace WeatherGrabber.Service
{
    public class Grabber
    {
        IStorageService _storage;
        private string _url;
        ILogger _logger;
        public TimeSpan Timeout;
        public Grabber(string url = null, IStorageService storage = null)
        {
            _logger = new LogFactory().GetLogger("WeatherGrabber");

            _url = !String.IsNullOrEmpty(url)
                ? url
                : ConfigurationManager.AppSettings["siteUrl"];

            _storage = storage ?? new MySqlStorageSerivce(ConfigurationManager.AppSettings["server"], ConfigurationManager.AppSettings["database"], ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["password"]);
            var timeout = ConfigurationManager.AppSettings["timeoutMin"];
            Timeout = Int32.TryParse(timeout, out var timeoutInt) ? TimeSpan.FromMinutes(timeoutInt) : TimeSpan.FromMinutes(10);
        }

        public async Task DoWork(CancellationToken token = default)
        {
            var data = GrabData(_url, token);

            try
            {
                await foreach (var d in data.WithCancellation(token))
                {
                    await _storage.InsertWeather(d);
                    _logger.Info($"City weather saved {d.Name}");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in city weather saving operation");
            }
        }

        public async IAsyncEnumerable<CityWeather> GrabData(string url = null, [EnumeratorCancellation]CancellationToken token = default)
        {
            var loader = new GrabberLoader();
            var listOfCities = await loader.GetPopularCityList(url, token);

            foreach (var path in listOfCities)
                yield return await loader.GetWeatherForCity(path, url, token);
        }
    }
}
