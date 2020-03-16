using System.Collections.Generic;

namespace WeatherGrabber.Service.Model
{
    public class Forecast
    {
        public WeatherDate Date { get; set; }
        public IEnumerable<Value> Values { get; set; }
    }
}
