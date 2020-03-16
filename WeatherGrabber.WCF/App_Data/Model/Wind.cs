using System.Runtime.Serialization;

namespace WeatherGrabber.WCF.Model
{
    [DataContract]
    public class Wind
    {
        [DataMember]
        public WindMeasurement Measurement { get; set; }
        [DataMember]
        public bool IsGust { get; set; }
        [DataMember]
        public string Value { get; set; }
        public Wind(WindMeasurement measurement, bool isGust, string value) 
        {
            Measurement = measurement;
            IsGust = isGust;
            Value = value;
        }
    }

    [DataContract]
    public enum WindMeasurement
    {
        [EnumMember]
        MS,
        [EnumMember]
        MiH,
        [EnumMember]
        KmH
    }
}
