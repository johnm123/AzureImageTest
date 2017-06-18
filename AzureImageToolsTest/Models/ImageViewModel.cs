using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace AzureImageToolsTest.Models
{
    public class ImageViewModel
    {
        public IEnumerable<Uri> ImageUris { get; }

        public bool HasUris => this.ImageUris != null && this.ImageUris.Any();

        public Uri CurrentImageUri
        {
            get
            {
                if (!ImageUris.Any())
                {
                    return null;
                }
               
                return this.ImageUris.ElementAt(CurrentImageNumber);
            }
        }

        public int CurrentImageNumber { get; }

        public string CurrentImageFileName
        {
            get
            {
                if (!ImageUris.Any())
                {
                    return string.Empty;
                }

                return ImageUris.ElementAt(this.CurrentImageNumber).Segments.Last();
            }
        }

        public bool CurrentImageIsFirstImage => this.CurrentImageNumber == 0;


        public bool CurrentImageIsLastImage
        {
            get
            {
                if (this.ImageUris != null && this.ImageUris.Any())
                {
                    return this.CurrentImageNumber == this.ImageUris.Count() - 1;
                }

                return true;
            }
        }

        public int PreviousImageNumber
        {
            get
            {
                if (this.CurrentImageNumber > 0)
                {
                    return this.CurrentImageNumber - 1;
                }

                return 0;
            }
        }

        public int NextImageNumber
        {
            get
            {
                if (this.CurrentImageNumber < int.MaxValue)
                {
                    return this.CurrentImageNumber + 1;
                }

                return int.MaxValue;
            }
        }

        public ImageViewModel(IEnumerable<Uri> imageUris, int currentImageNumber)
        {
            this.CurrentImageNumber = currentImageNumber;
            this.ImageUris = imageUris;
        }

        public ImageViewModel() { }
    }
}