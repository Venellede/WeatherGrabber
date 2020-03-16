using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeatherGrabber.App.WeatherGrabbberClient;

namespace WeatherGrabber.App
{
    public partial class MainForm : Form
    {
        WeatherGrabberServiceClient _client;
        ILogger _logger;
        Weather _weather;
        public MainForm()
        {
            _logger = new LogFactory().GetLogger("WeatherApplication");
            InitializeComponent();

            _client = new WeatherGrabberServiceClient();
            var temperatureType = new List<TypeView> 
            {
                new TypeView{ EnumType = TemperatureMeasurement.Celsius, View = "Цельсий", Text = "С" },
                new TypeView{ EnumType = TemperatureMeasurement.Fahrenheit, View = "Фаренгейт", Text = "F" }
            };
            cbTemperatureType.DataSource = new BindingSource(temperatureType, null);
            cbTemperatureType.DisplayMember = "View";

            var windType = new List<TypeView>
            {
                new TypeView{ EnumType = WindMeasurement.MS, View = "Метры в секунду", Text = "м/с" },
                new TypeView{ EnumType = WindMeasurement.KmH, View = "Километры в час", Text = "км/ч" },
                new TypeView{ EnumType = WindMeasurement.MiH, View = "Миль в час", Text = "миль/ч" }
            };
            cbWindType.DataSource = new BindingSource(windType, null);
            cbWindType.DisplayMember = "View";

            var pressureType = new List<TypeView>
            {
                new TypeView{ EnumType = PressureMeasurement.MmHgAtm, View = "Миллиметры ртутного столба", Text = "мм рт. ст." },
                new TypeView{ EnumType = PressureMeasurement.Pa, View = "Паскаль", Text = "кПа" },
                new TypeView{ EnumType = PressureMeasurement.InHg, View = "Дюйм ртутного столба", Text = "in Hg" },
            };
            cbPressureType.DataSource = new BindingSource(pressureType, null);
            cbPressureType.DisplayMember = "View";
        }

        public void WriteMessage()
        {
            if (_weather == null)
            {
                tbOutput.Text = @"Нет данных о погоде";
            }
            else
            {
                var temperatures = _weather.Temperatures.Where(t => t.Measurement == (TemperatureMeasurement)((TypeView)cbTemperatureType.SelectedItem).EnumType);
                var wind = _weather.Wind.FirstOrDefault(w => w.Measurement == (WindMeasurement)((TypeView)cbWindType.SelectedItem).EnumType && !w.IsGust)?.Value;
                var gust = _weather.Wind.FirstOrDefault(w => w.Measurement == (WindMeasurement)((TypeView)cbWindType.SelectedItem).EnumType && w.IsGust)?.Value;
                var pressures = _weather.Pressure.Where(p => p.Measurement == (PressureMeasurement)((TypeView)cbPressureType.SelectedItem).EnumType);
                var output = @"Погода в городе {0}

{1}
Средняя температрура {2} {5} (от {3} {5} до {4} {5})
Cкорость ветра {6} {7} {8}
Направление ветра {9}
Относительная влажность составит {10}% с осадками {11} мм 
Давление от {12} до {13} {14}
Ультрафиолетовый индекс равен {15} баллам
Геомогнитная активность {16} баллов";


                tbOutput.Text = String.Format(output,
                    cbCity.SelectedItem,
                    _weather.Tip,
                    temperatures.FirstOrDefault(t => t.RangeSide == RangeSide.Average)?.Value,
                    temperatures.FirstOrDefault(t => t.RangeSide == RangeSide.Min)?.Value,
                    temperatures.FirstOrDefault(t => t.RangeSide == RangeSide.Max)?.Value,
                    ((TypeView)cbTemperatureType.SelectedItem).Text,
                    wind,
                    ((TypeView)cbWindType.SelectedItem).Text,
                    String.IsNullOrEmpty(gust) ? "" : $"  с порывами до {gust} {((TypeView)cbWindType.SelectedItem).Text}",
                    _weather.WindDirection,
                    _weather.Humidity,
                    _weather.Precipitation,
                    pressures.FirstOrDefault(p => p.RangeSide == RangeSide.Min)?.Value,
                    pressures.FirstOrDefault(p => p.RangeSide == RangeSide.Max)?.Value,
                    ((TypeView)cbPressureType.SelectedItem).Text,
                    _weather.Uvb,
                    _weather.GeoM);
            }
            Refresh();
        }

        public async Task RefreshCities()
        {
            string[] cities;
            try
            {
                cities = await _client.GetCitiesAsync();
            }
            catch (Exception e)
            {
                _logger.Error(e, "Can't load city list");
                MessageBox.Show("Ошибка при подключению к серверу, получение данны невозможно.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            var selected = cbCity.SelectedItem;
            cbCity.Items.Clear();
            cbCity.Items.AddRange(cities);
            if (cities.Contains(selected))
                cbCity.SelectedItem = selected;

            Refresh();
        }

        public async Task GetWeather()
        {
            try
            {
                _weather = await _client.GetWeatherAsync(cbCity.SelectedItem?.ToString(), dtpDate.Value);
            }
            catch (FaultException e)
            {
                _logger.Error(e.Message);
                _weather = null;
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await RefreshCities();
        }

        private async void bRefresh_Click(object sender, EventArgs e)
        {
            await RefreshCities();
            await GetWeather();
            WriteMessage();
        }

        private async void bGet_Click(object sender, EventArgs e)
        {
            await GetWeather();
            WriteMessage();
        }

        private void cbTemperatureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteMessage();
        }

        private void cbWindType_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteMessage();
        }

        private void cbPressureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            WriteMessage();
        }
    }
}
