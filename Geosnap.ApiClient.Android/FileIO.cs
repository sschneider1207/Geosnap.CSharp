using System.Threading.Tasks;
using Geosnap.ApiClient.Interfaces;
using Java.IO;

namespace Geosnap.ApiClient.Android
{
    public class FileIO : IFileIO
    {
        public byte[] ReadAllBytes(string path)
        {
            using(var file = new File(path))
            using(var inputStream = new FileInputStream(file))
            {
                var buffer = new byte[file.Length()];
                inputStream.Read(buffer);
                return buffer;
            }
        }

        public async Task<byte[]> ReadAllBytesAsync(string path)
        {
            using (var file = new File(path))
            using (var inputStream = new FileInputStream(file))
            {
                var buffer = new byte[file.Length()];
                await inputStream.ReadAsync(buffer);
                return buffer;
            }
        }
    }
}