using System.Runtime.Serialization;

namespace WeatherGrabber.WCF.Model
{
    [DataContract]
    public enum RangeSide
    {
        [EnumMember]
        Min,
        [EnumMember]
        Max,
        [EnumMember]
        Average
    }
}
