using Newtonsoft.Json;

namespace Geosnap.ApiClient.Responses
{
    public sealed class ErrorResponse
    {
        public string Error { get; }

        [JsonConstructor]
        public ErrorResponse(string error)
        {
            Error = error;
        }
    }
}
