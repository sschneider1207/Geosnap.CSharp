using System.Threading.Tasks;
using Geosnap.Result;
using System.Net.Http;
using System;
using Geosnap.ApiClient.Requests;
using Newtonsoft.Json;
using System.Text;
using Geosnap.ApiClient.Responses;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Net.Http.Headers;
using Geosnap.ApiClient.Interfaces;

namespace Geosnap.ApiClient
{
    public class GeosnapClient : IGeosnapClient
    {
        private const string JsonMediaType = "application/json";
        private const string FormMediaType = "multipart/form-data";
        private Lazy<JsonSerializerSettings> SerializerSettings => new Lazy<JsonSerializerSettings>(() => new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        private Encoding Encoding => Encoding.UTF8;

        private readonly Uri _url;
        private readonly string _apiKey;
        private readonly IFileIO _fileIO;

        public GeosnapClient(string url, string apiKey, IFileIO fileIO)
        {
            _url = new Uri(url);
            _apiKey = apiKey;
            _fileIO = fileIO;
        }

        public async Task<Result<LoginResponse>> Login(string username, string password)
        {
            const string endpoint = "api/users/login";
            using(var client = GetClient())
            {
                var request = new LoginRequest(username, password);
                var json = JsonConvert.SerializeObject(request, SerializerSettings.Value);
                var requestContent = new StringContent(json, Encoding, JsonMediaType);
                HttpResponseMessage response;
                try {
                    response = await client.PostAsync(endpoint, requestContent);
                } catch (Exception e)
                {
                    return Result<LoginResponse>.Of(() => { throw e; });
                }
                var responseJson = await response.Content.ReadAsStringAsync();
                return Result<LoginResponse>.Of(() => TryGetResult<LoginResponse>(response.IsSuccessStatusCode, responseJson));
            }
        }

        public async Task<Result<UploadResponse>> UploadPicture(string authorization, string path, string title)
        {
            const string endpoint = "api/pictures";
            using (var client = GetAuthedClient(authorization))
            {
                byte[] bytes = null;
                Exception error = null;
                Result<byte[]>.Of(() => _fileIO.ReadAllBytes(path))
                    .Fold(b => bytes = b, e => error = e);
                if(error != null) { return Result<UploadResponse>.Error(error); }

                var content = new MultipartFormDataContent();

                var imageContent = new ByteArrayContent(bytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(imageContent, "picture", Path.GetFileName(path));

                var titleContent = new StringContent(title);
                content.Add(titleContent, "title");
                var response = await client.PostAsync(endpoint, content);
                var responseJson = await response.Content.ReadAsStringAsync();
                return Result<UploadResponse>.Of(() => TryGetResult<UploadResponse>(response.IsSuccessStatusCode, responseJson));
            }
        }

        public async Task<bool> AsyncUploadPicture(string authorization, string path, string title)
        {
            const string endpoint = "api/pictures-async";
            using (var client = GetAuthedClient(authorization))
            {
                byte[] bytes = null;
                Exception error = null;
                Result<byte[]>.Of(() => _fileIO.ReadAllBytes(path))
                    .Fold(b => bytes = b, e => error = e);
                if (error != null) { return false; }

                var content = new MultipartFormDataContent();

                var imageContent = new ByteArrayContent(bytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                content.Add(imageContent, "picture", Path.GetFileName(path));

                var titleContent = new StringContent(title);
                content.Add(titleContent, "title");
                var response = await client.PostAsync(endpoint, content);
                return response.IsSuccessStatusCode;
            }
        }

        private T TryGetResult<T>(bool IsSuccessStatusCode, string json)
        {
            if(!IsSuccessStatusCode)
            {
                var error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                throw new GeosnapException(error.Error);
            }
            return JsonConvert.DeserializeObject<T>(json);
        }    
        
        private HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = _url;
            client.DefaultRequestHeaders.Add("api-key", _apiKey);
            return client;
        }    

        private HttpClient GetAuthedClient(string authorization)
        {
            var client = new HttpClient();
            client.BaseAddress = _url;
            client.DefaultRequestHeaders.Add("api-key", _apiKey);
            client.DefaultRequestHeaders.Add("Authorization", authorization);
            return client;
        }
    }
}
