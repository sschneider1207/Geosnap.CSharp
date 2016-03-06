using Newtonsoft.Json;

namespace Geosnap.ApiClient.Responses
{
    public sealed class LoginResponse
    {
        public string Authorization { get; }
        public int Id { get; }

        [JsonConstructor]
        public LoginResponse(string authorization, int id)
        {
            Authorization = authorization;
            Id = id;
        }
    }
}
