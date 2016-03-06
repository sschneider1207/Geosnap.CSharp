using System.Threading.Tasks;

namespace Geosnap.ApiClient.Interfaces
{
    public interface IFileIO
    {
        byte[] ReadAllBytes(string path);
        Task<byte[]> ReadAllBytesAsync(string path);
    }
}
