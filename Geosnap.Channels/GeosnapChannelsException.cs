using System;

namespace Geosnap.Channels
{
    public class GeosnapChannelsException : Exception
    {
        public GeosnapChannelsException(string message) : base(message) { }

        public GeosnapChannelsException(string message, Exception inner) : base(message, inner) { }
    }
}
