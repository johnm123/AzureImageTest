using System;
using System.Collections.Generic;
using System.IO;

namespace AzureImageToolsTest.Storage
{
    public interface IFileStore
    {
        void Store(string fileName, Stream stream);

        IEnumerable<Uri> GetAllBlobPaths();

        void Delete(Uri uri);
    }
}