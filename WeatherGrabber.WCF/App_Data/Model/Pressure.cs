using System.Runtime.Serialization;

namespace WeatherGrabber.WCF.Model
{
    [DataContract]
    public class Pressure
    {
        [DataMember]
        public PressureMeasurement Measurement { get; private set; }
        [DataMember]
        public RangeSide RangeSide { get; private set; }
        [DataMember]
        public string Value { get; set; }

        public Pressure(PressureMeasurement measurement, RangeSide rangeSide, string value)
        {
            Measurement = measurement;
            RangeSide = rangeSide;
            Value = value;
        }
    }

    [DataContract]
    public enum PressureMeasurement
    {
        [EnumMember]
        MmHgAtm,
        [EnumMember]
        Pa,
        [EnumMember]
        InHg
    }
}
