using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureImageToolsTest.Models
{
    public class ImageViewModel
    {
        public IEnumerable<string> UriStrings { get; set; }

        public ImageViewModel(IEnumerable<Uri> uris)
        {
            this.UriStrings = uris.Select(uri => uri.AbsoluteUri).ToList();
        }
    }
}