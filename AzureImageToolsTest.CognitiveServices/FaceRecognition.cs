using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace AzureImageToolsTest.CognitiveServices
{
    public class FaceRecognition : IFaceRecognition
    {
        private readonly IFaceServiceClient _faceServiceClient;

        public FaceRecognition(FaceApiKey apiKey)
        {
            this._faceServiceClient = new FaceServiceClient(apiKey);
        }

        public FaceRectangle GetFirstFace(Stream inputStream)
        {
            var faces = this.UploadAndDetectFaces(inputStream);

            return faces.Any() ? faces.First() : new FaceRectangle();
        }

        private FaceRectangle[] UploadAndDetectFaces(Stream inputStream)
        {
            try
            {
                var faces = _faceServiceClient.DetectAsync(inputStream).Result;
                var faceRects = faces.Select(face => face.FaceRectangle);
                return faceRects.ToArray();
            }
            catch (Exception)
            {
                return new FaceRectangle[0];
            }
        }
    }
}
