using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeatherGrabber.WCF.Model
{
    [DataContract]
    public class Weather
    {
        [DataMember]
        public string Tip { get; set; }
        [DataMember]
        public IEnumerable<Temperature> Temperatures { get; set; }
        [DataMember]
        public IEnumerable<Wind> Wind { get; set; }
        [DataMember]
        public string WindDirection { get; set; }
        [DataMember]
        public string Precipitation { get; set; }
        [DataMember]
        public IEnumerable<Pressure> Pressure { get; set; }
        [DataMember]
        public string Humidity { get; set; }
        [DataMember]
        public string Uvb { get; set; }
        [DataMember]
        public string GeoM { get; set; }
    }
}