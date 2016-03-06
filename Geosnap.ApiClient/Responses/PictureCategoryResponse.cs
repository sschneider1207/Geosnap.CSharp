using Newtonsoft.Json;

namespace Geosnap.ApiClient.Responses
{
    public sealed class PictureCategoryResponse
    {
        public int Id { get; }
        public string Name { get; }

        [JsonConstructor]
        public PictureCategoryResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
