using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AzureImageToolsTest.CognitiveServices;
using AzureImageToolsTest.Storage;
using AzureImageToolsTest.Models;

namespace AzureImageToolsTest.Controllers
{
    [RoutePrefix("image")]
    [Route("{action = index}")] 
    public class ImageController : Controller
    {
        private readonly IFileStore _fileStore;

        private readonly IFaceRecognition _faceRecognition;

        public ImageController(IFileStore fileStore, IFaceRecognition faceRecognition)
        {
            this._fileStore = fileStore;
            this._faceRecognition = faceRecognition;
        }

        [HttpGet]
        [Route("")]
        public ActionResult Index(int imageNumber = 0)
        {
            var uris = this._fileStore.GetAllBlobPaths().ToArray();

            if (!uris.Any())
            {
                return View(new ImageViewModel());
            }

            return View(new ImageViewModel(uris, imageNumber));
        }

        [HttpPost]
        [Route("upload")]
        public ActionResult Upload(HttpPostedFileBase inputFile)
        {
            if (inputFile != null && inputFile.ContentLength > 0)
            {
                //var fileName = Path.GetFileName(inputFile.FileName);
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                this._fileStore.Store(inputFile.FileName, inputFile.InputStream);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("firstface")]
        public ActionResult FirstFace(Uri uri)
        {
            return View(new FaceViewModel());
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult Delete(Uri uri)
        {
            this._fileStore.Delete(uri);
  
            return RedirectToAction("Index");
        }
    }
}