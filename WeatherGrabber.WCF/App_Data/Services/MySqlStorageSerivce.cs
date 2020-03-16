using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using WeatherGrabber.WCF.Faults;
using WeatherGrabber.WCF.Model;

namespace WeatherGrabber.WCF.Services
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

        public async Task<IEnumerable<string>> GetCities(CancellationToken token = default)
        {
            using var connection = new MySqlConnection(_sqlBuilder.ConnectionString);
            await connection.OpenAsync(token);
            var sql = @"select distinct `Name` from cities order by 1;";
            using var command = new MySqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync(token);
            List<string> result = new List<string>();
            while (await reader.ReadAsync(token))
            {
                result.Add(reader.GetString(0));
            }
            return result;
        }

        public async Task<IEnumerable<DateTime>> GetDatesForCity(string cityName, CancellationToken token = default)
        {
            using var connection = new MySqlConnection(_sqlBuilder.ConnectionString);
            await connection.OpenAsync(token);
            var sql = @"
select distinct `Date` 
from cities c
join forecast f on f.CityId = c.Id
where c.`Name` = @cityName 
order by 1;";
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("cityName", cityName);
            using var reader = await command.ExecuteReaderAsync(token);
            List<DateTime> result = new List<DateTime>();
            while (await reader.ReadAsync(token))
            {
                result.Add(reader.GetDateTime(0));
            }
            return result;
        }

        public async Task<Weather> GetWeather(string cityName, DateTime dateTime, CancellationToken token = default)
        {
            using var connection = new MySqlConnection(_sqlBuilder.ConnectionString);
            await connection.OpenAsync(token);
            var sql = @"
select 
MAX(case when v.ValueTypeId = 2 then v.`Value` end) as 'Tip',
MAX(case when v.ValueTypeId = 3 then v.`Value` end) as 'C max',
MAX(case when v.ValueTypeId = 4 then v.`Value` end) as 'F max',
MAX(case when v.ValueTypeId = 5 then v.`Value` end) as 'C min',
MAX(case when v.ValueTypeId = 6 then v.`Value` end) as 'F min',
MAX(case when v.ValueTypeId = 7 then v.`Value` end) as 'Wind ms',
MAX(case when v.ValueTypeId = 8 then v.`Value` end) as 'Wind MiH',
MAX(case when v.ValueTypeId = 9 then v.`Value` end) as 'Wind KmH',
MAX(case when v.ValueTypeId = 10 then v.`Value` end) as 'Prep',
MAX(case when v.ValueTypeId = 11 then v.`Value` end) as 'C Average',
MAX(case when v.ValueTypeId = 12 then v.`Value` end) as 'F Average',
MAX(case when v.ValueTypeId = 13 then v.`Value` end) as 'W Direction',
MAX(case when v.ValueTypeId = 14 then v.`Value` end) as 'Gust ms',
MAX(case when v.ValueTypeId = 15 then v.`Value` end) as 'Gust mih',
MAX(case when v.ValueTypeId = 16 then v.`Value` end) as 'Gust kmh',
MAX(case when v.ValueTypeId = 19 then v.`Value` end) as 'Humidity',
MAX(case when v.ValueTypeId = 20 then v.`Value` end) as 'Uvb',
MAX(case when v.ValueTypeId = 21 then v.`Value` end) as 'GeoM',
MAX(case when v.ValueTypeId = 22 then v.`Value` end) as 'mm_hg_atm min',
MAX(case when v.ValueTypeId = 23 then v.`Value` end) as 'mm_hg_atm max',
MAX(case when v.ValueTypeId = 24 then v.`Value` end) as 'h_pa min',
MAX(case when v.ValueTypeId = 25 then v.`Value` end) as 'h_pa max',
MAX(case when v.ValueTypeId = 26 then v.`Value` end) as 'in_hg min',
MAX(case when v.ValueTypeId = 27 then v.`Value` end) as 'in_hg max'
from value v
join
 (
     select c.`Name`, f.Date, f.`Id`
     from cities c
     join forecast f on f.CityId = c.id
     where c.`Name` = @cityName and f.Date = @dateTime
     order by f.Created desc
     limit 1
 ) as d on d.`Id` = v.ForecastId
group by d.`Name`, d.`Date`;";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("cityName", cityName);
            command.Parameters.AddWithValue("dateTime", dateTime.ToString("yyyy-MM-dd"));
            using var reader = await command.ExecuteReaderAsync(token);
            List<DateTime> result = new List<DateTime>();
            if (!await reader.ReadAsync(token))
                throw new FaultException<WeatherNotFoundFault>(new WeatherNotFoundFault(), "Weather not found");

            return new Weather
            {
                Tip = reader.IsDBNull(0) ? null : reader.GetString(0),
                Temperatures = new[]
                {
                    new Temperature(TemperatureMeasurement.Celsius, RangeSide.Max, reader.IsDBNull(1) ? null : reader.GetString(1)),
                    new Temperature(TemperatureMeasurement.Fahrenheit, RangeSide.Max, reader.IsDBNull(2) ? null : reader.GetString(2)),
                    new Temperature(TemperatureMeasurement.Celsius, RangeSide.Min, reader.IsDBNull(3) ? null : reader.GetString(3)),
                    new Temperature(TemperatureMeasurement.Fahrenheit, RangeSide.Min, reader.IsDBNull(4) ? null : reader.GetString(4)),
                    new Temperature(TemperatureMeasurement.Celsius, RangeSide.Average, reader.IsDBNull(9) ? null : reader.GetString(9)),
                    new Temperature(TemperatureMeasurement.Fahrenheit, RangeSide.Average, reader.IsDBNull(10) ? null : reader.GetString(10))
                },
                WindDirection = reader.IsDBNull(11) ? null : reader.GetString(11),
                Wind = new[]
                {
                    new Wind(WindMeasurement.MS, false, reader.IsDBNull(5) ? null : reader.GetString(5)),
                    new Wind(WindMeasurement.MiH, false, reader.IsDBNull(6) ? null : reader.GetString(6)),
                    new Wind(WindMeasurement.KmH, false, reader.IsDBNull(7) ? null : reader.GetString(7)),
                    new Wind(WindMeasurement.MS, true, reader.IsDBNull(12) ? null : reader.GetString(12)),
                    new Wind(WindMeasurement.MiH, true, reader.IsDBNull(13) ? null : reader.GetString(13)),
                    new Wind(WindMeasurement.KmH, true, reader.IsDBNull(14) ? null : reader.GetString(14)),
                },
                Precipitation = reader.IsDBNull(8) ? null : reader.GetString(8),
                Humidity = reader.IsDBNull(15) ? null : reader.GetString(15),
                Uvb = reader.IsDBNull(16) ? null : reader.GetString(16),
                GeoM = reader.IsDBNull(17) ? null : reader.GetString(17),
                Pressure = new[]
                {
                    new Pressure(PressureMeasurement.MmHgAtm, RangeSide.Min, reader.IsDBNull(18) ? null : reader.GetString(18)),
                    new Pressure(PressureMeasurement.MmHgAtm, RangeSide.Max, reader.IsDBNull(19) ? null : reader.GetString(19)),
                    new Pressure(PressureMeasurement.Pa, RangeSide.Min, reader.IsDBNull(20) ? null : reader.GetString(20)),
                    new Pressure(PressureMeasurement.Pa, RangeSide.Max, reader.IsDBNull(21) ? null : reader.GetString(21)),
                    new Pressure(PressureMeasurement.InHg, RangeSide.Min, reader.IsDBNull(22) ? null : reader.GetString(22)),
                    new Pressure(PressureMeasurement.InHg, RangeSide.Max, reader.IsDBNull(23) ? null : reader.GetString(23)),
                }
            };
        }
    }
}
