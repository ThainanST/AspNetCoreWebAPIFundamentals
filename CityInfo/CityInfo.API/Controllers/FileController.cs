using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileETP;

        public FileController(FileExtensionContentTypeProvider fileETP)
        {
            _fileETP = fileETP ?? throw new System.ArgumentNullException(nameof(fileETP));
        }

        [HttpGet("{fileId}")]
        public ActionResult GetFile(int fileId)
        {
            var pathToFile = "pdf-file.pdf";
            if (!System.IO.File.Exists(pathToFile)) return NotFound();
            if(!_fileETP.TryGetContentType(pathToFile, out var contentType)) 
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }
    }
}
