using Geosnap.Channels.Interfaces;
using Newtonsoft.Json.Linq;
using Phoenix;
using System;

namespace Geosnap.Channels
{
    public class GeosnapChannels : IGeosnapChannels
    {
        private readonly Socket _socket;
        
        private Channel _userChannel;
        private readonly object _userChannelLock = new object();
        private Channel UserChannel
        {
            get
            {
                if(_userChannel == null) { throw new GeosnapChannelsException("User channel has not yet been joined."); }
                return _userChannel;
            }
        }
        
        public GeosnapChannels(string endpoint, string jwt)
        {
            if (!endpoint.StartsWith("ws://") && !endpoint.StartsWith("wss://")) { throw new GeosnapChannelsException("Endpoint protocol must be ws/wss."); }
            var options = new SocketOptions
            {
                //LogCallback = Logger,
                Params = new JObject { { "jwt", jwt } }
            };
            _socket = new Socket(endpoint, options);
            _socket.Connect();
        }

        public void JoinUserChannel(int userId, Action successCallback, Action<Exception> errorCallback)
        {
            lock(_userChannelLock)
            {
                if(_userChannel != null) { _userChannel.Leave(); }

                _userChannel = _socket.Channel($"users:{userId}", null);

                _userChannel.Join()
                    .Receive("ok", (_ => successCallback()))
                    .Receive("error", (msg => errorCallback(new GeosnapChannelsException($"Unable to join channel: {msg.ToString()}"))))
                    .Receive("timeout", (_ => errorCallback(new GeosnapChannelsException($"Timed out trying to join topic users:{userId}"))));
            }
        }

        public void OnUploadComplete(Action<JObject, string> callback) => UserChannel.On("upload_complete", callback);

        //public void Logger(string kind, string msg, JObject data = null) => Console.WriteLine($"{kind} - {msg}");
    }
}
