using System.Threading;
using System.Threading.Tasks;
using WeatherGrabber.Service.Model;

namespace WeatherGrabber.Service.Services
{
    public interface IStorageService
    {
        Task InsertWeather(CityWeather weather, CancellationToken token = default);
    }
}
