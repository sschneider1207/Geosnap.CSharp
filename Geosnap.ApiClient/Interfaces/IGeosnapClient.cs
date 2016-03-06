using Geosnap.ApiClient.Responses;
using Geosnap.Result;
using System.Threading.Tasks;

namespace Geosnap.ApiClient.Interfaces
{
    public interface IGeosnapClient
    {
        Task<Result<LoginResponse>> Login(string username, string password);
        Task<Result<UploadResponse>> UploadPicture(string authorization, string path, string title);
        Task<bool> AsyncUploadPicture(string authorization, string path, string title);
    }
}
