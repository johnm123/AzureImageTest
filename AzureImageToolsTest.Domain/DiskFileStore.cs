using System;
using System.IO;
using Serilog;

namespace AzureImageToolsTest.Domain
{
    public class DiskFileStore : IFileStore
    {
        private const int MaxFileSizeInMb = 3;
        private readonly DiskFileStoreRoot _diskFileStoreRoot;
        private readonly ILogger _logger;

        public DiskFileStore(DiskFileStoreRoot diskFileStoreRoot, ILogger logger)
        {
            _diskFileStoreRoot = diskFileStoreRoot;
            _logger = logger;
            this.EnsureFileStoreRootExists();
        }

        public void StoreFileFromStream(string fileName, Stream stream)
        {     
            if (stream == null || !stream.CanRead)
            {
                _logger.Debug($"Empty stream for {fileName}.");
                throw new FileLoadException("Could not store stream.");
            }

            if (stream.Length > GetMaxFileSizInBytes())
            {
                _logger.Debug($"Stream too long for '{fileName}', {stream.Length}B.");
                throw new FileLoadException($"File exceeds maximum size of {MaxFileSizeInMb}MB.");
            }

            string filePath = GetFileStorePath(fileName);
            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }
                _logger.Information( $"Successfully saved file at path '{filePath}'.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Could not save file at path '{filePath}'.");
                throw new DiskFileStoreException();
            }
        }

        private string GetFileStorePath(string fileName)
        {
            string pathRoot = _diskFileStoreRoot;
            return $"{pathRoot}" +
                   $"{Path.GetFileNameWithoutExtension(fileName)}" +
                   $"{Guid.NewGuid()}" +
                   $"{Path.GetExtension(fileName)}";
        }

        private void EnsureFileStoreRootExists()
        {
            try
            {
                Directory.CreateDirectory(_diskFileStoreRoot);
            }
            catch (Exception ex)
            {
                string rootPath = _diskFileStoreRoot;
                _logger.Error(ex, $"Could not create directory at path '{rootPath}'.");
                throw new DiskFileStoreException();
            }
        }

        private static int GetMaxFileSizInBytes()
        {
            return MaxFileSizeInMb * 1048576;
        }
    }

    internal class DiskFileStoreException : Exception
    {

    }
}
