using System.IO;

namespace AzureImageToolsTest.Domain
{
    public interface IFileStore
    {
        void StoreFileFromStream(string fileName, Stream stream);
    }
}