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

        [HttpPost]
        public async Task<ActionResult> UploadFile(IFormFile file)
        {
            // Validate the input. Size, extension, etc.
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length > 10 * 1024 * 1024) return BadRequest("File grater than 2MB.");
            if (file.ContentType != "application/pdf") return BadRequest("no pdf");

            // Avoid using file.FileName directly. It can be dangerous.
            // Avoid using full path or relative path. It can be dangerous.
            // Use safe locations without execution privileges. 
            var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    $"uploaded_file_{Guid.NewGuid()}.pdf"
                );

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your file has been successfully uploaded");
        }
    }
}
