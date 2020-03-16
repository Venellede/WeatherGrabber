using System.Runtime.Serialization;

namespace WeatherGrabber.WCF.Model
{
    [DataContract]
    public class Temperature
    {
        [DataMember]
        public TemperatureMeasurement Measurement { get; private set; }
        [DataMember]
        public RangeSide RangeSide { get; private set; }
        [DataMember]
        public string Value { get; set; }

        public Temperature(TemperatureMeasurement measurement, RangeSide rangeSide, string value)
        {
            Measurement = measurement;
            RangeSide = rangeSide;
            Value = value;
        }
    }

    [DataContract]
    public enum TemperatureMeasurement
    {
        [EnumMember]
        Celsius,
        [EnumMember]
        Fahrenheit
    }
}
