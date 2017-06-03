using System;
using System.IO;
using Serilog;

namespace AzureImageToolsTest.Domain
{
    public class DiskFileStore : IFileStore
    {
        private readonly ILogger _logger;

        public DiskFileStore(ILogger logger)
        {
            _logger = logger;
        }

        private static readonly string FileSaveRoot = Directory.GetCurrentDirectory();

        public void StoreFileFromStream(string fileName, Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                throw new FileLoadException("Could not store stream");

            }

            string filePath = $"{FileSaveRoot}{Path.DirectorySeparatorChar}{fileName}";
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Could not save file at path {filePath}");
            }
        }
    }
}
