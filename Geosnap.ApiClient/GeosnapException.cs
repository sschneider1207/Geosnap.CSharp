using System;
namespace Geosnap.ApiClient
{
    public class GeosnapException : Exception
    {
        public GeosnapException(string message) : base(message) { }

        public GeosnapException(string message, Exception innerException) : base(message, innerException) { }
    }
}
