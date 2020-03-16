using MySql.Data.MySqlClient;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeatherGrabber.Service.Model;

namespace WeatherGrabber.Service.Services
{
    public class MySqlStorageSerivce : IStorageService
    {
        private readonly MySqlConnectionStringBuilder _sqlBuilder;
        public MySqlStorageSerivce(string server, string database, string user, string password)
        {
            _sqlBuilder = new MySqlConnectionStringBuilder();
            _sqlBuilder.Server = server;
            _sqlBuilder.Database = database;
            _sqlBuilder.UserID = user;
            _sqlBuilder.Password = password;
            _sqlBuilder.CharacterSet = "utf8";
        }

        public async Task InsertWeather(CityWeather weather, CancellationToken token = default)
        {
            int cityId;
            using var connection = new MySqlConnection(_sqlBuilder.ConnectionString);
            await connection.OpenAsync(token);
            using var transaction = await connection.BeginTransactionAsync(token);
            var createForecast = @"
INSERT IGNORE INTO cities (`Name`)  VALUES ( @cityName );
SELECT id FROM cities WHERE `Name` = @cityName;";
            using (var command = new MySqlCommand(createForecast, connection, transaction))
            {
                command.Parameters.AddWithValue("cityName", weather.Name);

                cityId = (int)await command.ExecuteScalarAsync(token);
            }

            async Task<uint> AddForecast(DateTime dateTime, int city)
            {
                var addForecast = $@"
INSERT INTO forecast (`Date`, `CityId`) VALUES (@dateTime, @city);
SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(addForecast, connection, transaction))
                {
                    command.Parameters.AddWithValue("city", city);
                    command.Parameters.AddWithValue("dateTime", dateTime);

                    var result = await command.ExecuteScalarAsync(token);
                    return uint.Parse(result.ToString());
                }
            }

            async Task AddValues(uint forecast, int valueType, string value)
            {
                var addValue = "INSERT INTO `value` (`ForecastId`, `ValueTypeId`, `Value`) VALUES (@forecastId, @valueTypeId, @value);";
                using (var command = new MySqlCommand(addValue, connection, transaction))
                {
                    command.Parameters.AddWithValue("forecastId", forecast);
                    command.Parameters.AddWithValue("valueTypeId", valueType);
                    command.Parameters.AddWithValue("value", value);

                    await command.ExecuteNonQueryAsync(token);
                }
            }

            foreach (var date in weather.Forecast)
            {
                var forecast = await AddForecast(date.Date.DateTime, cityId);
                foreach (var value in date.Values)
                    await AddValues(forecast, (int)value.Type, value.Data);
            }

            transaction.Commit();
        }
    }
}
