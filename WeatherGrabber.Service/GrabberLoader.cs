using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using WeatherGrabber.Service.Model;
using ValueType = WeatherGrabber.Service.Model.ValueType;

namespace WeatherGrabber.Service
{
    public class GrabberLoader
    {
        private readonly HtmlWeb _client = new HtmlWeb();
        private const string gismeteoUri = "https://www.gismeteo.ru";

        public async Task<string[]> GetPopularCityList(string uri = null, CancellationToken token = default)
        {
            var mainPage = await _client.LoadFromWebAsync(uri ?? gismeteoUri, token);

            return mainPage
                .DocumentNode
                .Descendants("noscript")
                .FirstOrDefault(d => d.Attributes["id"]?.Value == "noscript")
                ?.Descendants("a")
                .Select(d => d.Attributes["data-url"]?.Value)
                .ToArray();
        }

        public async Task<CityWeather> GetWeatherForCity(string path, string uri = null, CancellationToken token = default)
        {
            var url = $"{uri?.TrimEnd('/') ?? gismeteoUri}/{path.Trim('/')}/10-days/";
            var mainPage = await _client.LoadFromWebAsync(url, token);

            var cityName = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "subnav_search_city js_citytitle").InnerText;
            var widget = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "widget__container");
            string month = "";
            var dates = widget.Descendants("div").Where(d => d.Attributes["class"]?.Value == "w_date")
                .Select(d =>
                {
                    var dayOfWeek = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "w_date__day")?.InnerText.Trim();
                    var date = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "w_date__date black" || a.Attributes["class"]?.Value == "w_date__date weekend")?.InnerText.Replace("\n", "").Trim().Split(' ');
                    if (date.Length == 2)
                        month = date[1];
                    Int32.TryParse(date[0], out var day);
                    return new WeatherDate { DayOfWeek = dayOfWeek, Month = month, Day = day };
                }).ToArray();

            token.ThrowIfCancellationRequested();

            var tips = widget.Descendants("div").Where(d => d.Attributes["class"]?.Value == "widget__value w_icon")
                .Select(a => a.Descendants("span").FirstOrDefault(d => d.Attributes["class"]?.Value == "tooltip")?.Attributes["data-text"]?.Value)
                .Select(a => new Value(a, ValueType.Tip))
                .ToArray();

            token.ThrowIfCancellationRequested();

            var temperatures = widget.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "templine w_temperature")
                .Descendants("div")
                .FirstOrDefault(d => d.Attributes["class"]?.Value == "values").ChildNodes
                .Select(d =>
                {

                    var maxC = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "maxt")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_c")?.InnerText.Trim().Replace("&minus;", "-");
                    var maxF = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "maxt")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_f")?.InnerText.Trim().Replace("&minus;", "-");

                    var minC = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_c")?.InnerText.Trim().Replace("&minus;", "-");
                    var minF = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_f")?.InnerText.Trim().Replace("&minus;", "-");

                    return new[]
                    {
                        new Value(maxC, ValueType.Cmax),
                        new Value(maxF, ValueType.Fmax),
                        new Value(minC, ValueType.Cmin),
                        new Value(minF, ValueType.Fmin)

                    };
                }).ToArray();

            token.ThrowIfCancellationRequested();

            var mediumTemperatures = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "widget js_widget" && d.Attributes["data-widget-id"]?.Value == "averageTemp")
                .Descendants("div").Where(d => d.Attributes["class"]?.Value == "value")
                .Select(d =>
                {
            
                    var averageC = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_c")?.InnerText.Trim().Replace("&minus;", "-");
                    var averageF = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_f")?.InnerText.Trim().Replace("&minus;", "-");
            
                    var minC = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_c")?.InnerText.Trim().Replace("&minus;", "-");
                    var minF = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_temperature_f")?.InnerText.Trim().Replace("&minus;", "-");
            
                    return new[]
                    {
                        new Value(averageC, ValueType.Caverage),
                        new Value(averageF, ValueType.Faverage)
            
                    };
                }).ToArray();

            token.ThrowIfCancellationRequested();

            var windWidget = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "widget js_widget" && d.Attributes["data-widget-id"]?.Value == "wind");

            var wind = windWidget.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "widget__row widget__row_table widget__row_wind").ChildNodes
                .Select(d =>
                {
                    var windms = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_wind_m_s")?.InnerText.Trim();
                    var windmih = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_wind_mi_h")?.InnerText.Trim();
                    var windkmh = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_wind_km_h")?.InnerText.Trim();
                    var winddirection = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value.Contains("w_wind__direction") == true)?.InnerText.Trim();
                    return new[]
                    {
                        new Value(windms, ValueType.WFms),
                        new Value(windmih, ValueType.WFmih),
                        new Value(windkmh, ValueType.WFkmh),
                        new Value(winddirection, ValueType.Wdirection)
                    };
                })
                .ToArray();

            token.ThrowIfCancellationRequested();

            var gust = windWidget.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "widget__row widget__row_table widget__row_gust").ChildNodes
                .Select(d =>
                {
                    var gustms = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_wind_m_s")?.InnerText.Replace("&mdash;", "").Trim();
                    var gustmih = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_wind_mi_h")?.InnerText.Replace("&mdash;", "").Trim();
                    var gustkmh = d.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_wind_km_h")?.InnerText.Replace("&mdash;", "").Trim();
                    return new[]
                    {
                        new Value(String.IsNullOrEmpty(gustms) ? null : gustms, ValueType.GustMs),
                        new Value(String.IsNullOrEmpty(gustmih) ? null : gustmih, ValueType.GustMih),
                        new Value(String.IsNullOrEmpty(gustkmh) ? null : gustkmh, ValueType.GustKmh)
                    };
                })
                .ToArray();

            token.ThrowIfCancellationRequested();

            var precipitation = widget.Descendants("div").Where(d => d.Attributes["class"]?.Value == "w_prec__value").Select(d =>
            {
                return new Value(d.InnerText.Trim(), ValueType.Precipitation);
            }).ToArray();

            token.ThrowIfCancellationRequested();

            var pressure = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "chart chart__pressure")
                .Descendants("div")
                .FirstOrDefault(d => d.Attributes["class"]?.Value == "values").ChildNodes
                .Select(d =>
                {

                    var maxPressureMm = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "maxt")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_pressure_mm_hg_atm")?.InnerText.Trim();
                    var maxPressurePa = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "maxt")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_pressure_h_pa")?.InnerText.Trim();
                    var maxPressureIn = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "maxt")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_pressure_in_hg")?.InnerText.Trim();

                    var minPressureMm = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_pressure_mm_hg_atm")?.InnerText.Trim();
                    var minPressurePa = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_pressure_h_pa")?.InnerText.Trim();
                    var minPressureIn = d.Descendants("div").FirstOrDefault(a => a.Attributes["class"]?.Value == "mint")
                        ?.Descendants("span").FirstOrDefault(a => a.Attributes["class"]?.Value == "unit unit_pressure_in_hg")?.InnerText.Trim();

                    return new[]
                    {
                        new Value(minPressureMm, ValueType.PressureMmMin),
                        new Value(maxPressureMm, ValueType.PressureMmMax),
                        new Value(minPressurePa, ValueType.PressurePaMin),
                        new Value(maxPressurePa, ValueType.PressurePaMax),
                        new Value(minPressureIn, ValueType.PressureInMin),
                        new Value(maxPressureIn, ValueType.PressureInMax)

                    };
                }).ToArray();

            token.ThrowIfCancellationRequested();

            var humidity = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "widget__row widget__row_table widget__row_humidity").ChildNodes.Select(d =>
            {
                return new Value(d.ChildNodes.First().InnerText.Trim(), ValueType.Humidity);
            }).ToArray();

            token.ThrowIfCancellationRequested();

            var uvb = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "widget__row widget__row_table widget__row_uvb").ChildNodes.Select(d =>
            {
                return new Value(d.ChildNodes.First().InnerText.Trim(), ValueType.Uvb);
            }).ToArray();

            token.ThrowIfCancellationRequested();

            var geom = mainPage.DocumentNode.Descendants("div").FirstOrDefault(d => d.Attributes["class"]?.Value == "widget__row widget__row_table widget__row_gm").ChildNodes.Select(d =>
            {
                return new Value(d.ChildNodes.First().InnerText.Trim(), ValueType.GeoM);
            }).ToArray();

            token.ThrowIfCancellationRequested();


            var result = new CityWeather();
            result.Name = cityName;
            var forecasts = new List<Forecast>();
            for (int i = 0; i < dates.Length; i++)
            {
                var values = new List<Value>();
                values.Add(tips[i]);
                values.AddRange(temperatures[i]);
                values.AddRange(mediumTemperatures[i]);
                values.AddRange(wind[i]);
                values.AddRange(gust[i]);
                values.Add(precipitation[i]);
                values.AddRange(pressure[i]);
                values.Add(humidity[i]);
                values.Add(uvb[i]);
                values.Add(geom[i]);

                forecasts.Add(new Forecast { Date = dates[i], Values = values });
            }
                
            result.Forecast = forecasts;
            return result;
        }
    }
}
