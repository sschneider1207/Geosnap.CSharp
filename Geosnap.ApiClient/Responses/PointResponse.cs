using Newtonsoft.Json;

namespace Geosnap.ApiClient.Responses
{
    public struct PointResponse
    {
        public double Longitude { get; }
        public double Latitude { get; }

        [JsonConstructor]
        public PointResponse(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
