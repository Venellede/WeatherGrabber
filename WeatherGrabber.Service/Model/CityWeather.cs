using System.Collections.Generic;

namespace WeatherGrabber.Service.Model
{
    public class CityWeather
    {
        public string Name { get; set; }
        public IEnumerable<Forecast> Forecast { get; set; }
    }
}
