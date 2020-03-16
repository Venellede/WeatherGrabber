using System;
using WeatherGrabber.Service.Exceptions;

namespace WeatherGrabber.Service.Model
{
    public class WeatherDate
    {
        public string Month { get; set; }
        public int Day { get; set; }
        public string DayOfWeek { get; set; }

        public DateTime DateTime => new DateTime(DateTime.Now.Year, IntMonth, Day); 

        private int IntMonth
        {
            get
            {
                switch (Month)
                {
                    case "янв":
                        return 1;
                    case "фев":
                        return 2;
                    case "мар":
                        return 3;
                    case "апр":
                        return 4;
                    case "май":
                        return 5;
                    case "июн":
                        return 6;
                    case "июл":
                        return 7;
                    case "авг":
                        return 8;
                    case "сен":
                        return 9;
                    case "окт":
                        return 10;
                    case "ноя":
                        return 11;
                    case "дек":
                        return 12;
                    default:
                        throw new InvlidWeatherDateMonthNamingException();
                }
            }
        }
    }
}
