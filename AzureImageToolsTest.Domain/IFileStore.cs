using System;
using System.Collections.Generic;
using System.IO;

namespace AzureImageToolsTest.Domain
{
    public interface IFileStore
    {
        void Store(string fileName, Stream stream);

        IEnumerable<Uri> GetAllBlobPaths();
    }
}