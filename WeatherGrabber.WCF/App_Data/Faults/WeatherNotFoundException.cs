using System.Runtime.Serialization;

namespace WeatherGrabber.WCF.Faults
{
    [DataContract]
    public class WeatherNotFoundFault
    {
        [DataMember]
        public string CustomError;
        public WeatherNotFoundFault()
        {
        }
        public WeatherNotFoundFault(string error)
        {
            CustomError = error;
        }
    }
}