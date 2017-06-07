using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob; 
namespace AzureImageToolsTest.Domain
{
    public class BlobFileStore : IFileStore
    {
        private const int MaxFileSizeInMb = 3;
        private const string BlobContainerName = "mycontainer";

        private readonly ILogger _logger;
        private readonly StorageConnectionString _storageConnectionString;

        public BlobFileStore(ILogger logger, StorageConnectionString storageConnectionString)
        {
            _logger = logger;
            _storageConnectionString = storageConnectionString;
        }

        public void Store(string fileName, Stream stream)
        {
            if (stream == null || !stream.CanRead)
            {
                throw new FileLoadException("Could not store stream");
            }

            if (stream.Length > GetMaxFileSizInBytes())
            {
                _logger.Debug($"Stream too long for '{fileName}', {stream.Length}B.");
                throw new FileLoadException($"File exceeds maximum size of {MaxFileSizeInMb}MB.");
            }

            var uniqueFilename = GetUniqueFilename(fileName);

            var blockBlob = GetBlockBlobReference(uniqueFilename);

            try
            {
                blockBlob.UploadFromStream(stream);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Could not store blob at {uniqueFilename}.");
                throw new BlobStoreException();
            }
        }

        public IEnumerable<Uri> GetAllBlobPaths()
        {
            var container = GetBlobContainerReference();

            var blobs = container.ListBlobs();

            return blobs.Select(blobItem => blobItem.Uri).ToArray();
        }

        private CloudBlockBlob GetBlockBlobReference(string fileName)
        {
            try
            {
                var container = this.GetBlobContainerReference();

                container.CreateIfNotExists();

                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                return container.GetBlockBlobReference(fileName); 
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Could not get block blob reference.");
                throw new BlobConfigurationException();
            }
        }

        private CloudBlobContainer GetBlobContainerReference()
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(this._storageConnectionString);

                var blobClient = storageAccount.CreateCloudBlobClient();

                var container = blobClient.GetContainerReference(BlobContainerName);

                return container;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Could not get container reference.");
                throw new BlobConfigurationException();
            }
        }

        private static int GetMaxFileSizInBytes()
        {
            return MaxFileSizeInMb * 1048576;
        }

        private static string GetUniqueFilename(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}" +
                   $"{Guid.NewGuid()}" +
                   $"{Path.GetExtension(fileName)}";
        }
    }
}
