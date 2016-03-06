using Newtonsoft.Json;

namespace Geosnap.ApiClient.Responses
{
    public struct UserResponse
    {
        public int Id { get; }

        [JsonConstructor]
        public UserResponse(int id)
        {
            Id = id;
        }
    }
}
