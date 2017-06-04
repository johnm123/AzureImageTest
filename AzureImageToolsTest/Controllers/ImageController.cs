using System.Web;
using System.Web.Mvc;
using AzureImageToolsTest.Domain;

namespace AzureImageToolsTest.Controllers
{
    [RoutePrefix("image")]
    [Route("{action = index}")] 
    public class ImageController : Controller
    {
        private readonly IFileStore _fileStore;

        public ImageController(IFileStore fileStore)
        {
            _fileStore = fileStore;
        }

        [HttpGet]
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("upload")]
        public ActionResult Upload(HttpPostedFileBase inputFile)
        {
            if (inputFile.ContentLength > 0)
            {
                //var fileName = Path.GetFileName(inputFile.FileName);
                //var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                _fileStore.StoreFileFromStream(inputFile.FileName, inputFile.InputStream);
            }

            return RedirectToAction("Index");
        }
    }
}