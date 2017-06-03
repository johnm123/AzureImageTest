using System.IO;

namespace AzureImageToolsTest.Domain
{
    public class BlobFileStore : IFileStore
    {
        public void StoreFileFromStream(string fileName, Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                throw new FileLoadException("Could not store stream");
            }
        }
    }
}
