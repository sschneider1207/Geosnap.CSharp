using Newtonsoft.Json.Linq;
using System;

namespace Geosnap.Channels.Interfaces
{
    public interface IGeosnapChannels
    {
        void JoinUserChannel(int userId, Action successCallback, Action<Exception> errorCallback);
        void OnUploadComplete(Action<JObject, string> callback);
        //void Logger(string kind, string msg, JObject data = null);
    }
}
