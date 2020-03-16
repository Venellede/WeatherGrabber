using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherGrabber.Service.Model
{
    public class Value
    {
        public string Data { get; set; }
        public ValueType Type { get; set; }

        public Value(string value, ValueType type)
        {
            Data = value;
            Type = type;
        }
    }
}
