using System.IO;
using Microsoft.ProjectOxford.Face.Contract;

namespace AzureImageToolsTest.CognitiveServices
{
    public interface IFaceRecognition
    {
        FaceRectangle GetFirstFace(Stream inputStream);
    }
}