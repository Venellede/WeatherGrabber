using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using WeatherGrabber.WCF.Faults;
using WeatherGrabber.WCF.Model;

namespace WeatherGrabber.WCF
{
    [ServiceContract]
    public interface IWeatherGrabberService
    {
        [OperationContract]
        Task<IEnumerable<string>> GetCities();
        [OperationContract]
        [FaultContract(typeof(WeatherNotFoundFault))]
        Task<Weather> GetWeather(string cityName, DateTime dateTime);
    }
}
