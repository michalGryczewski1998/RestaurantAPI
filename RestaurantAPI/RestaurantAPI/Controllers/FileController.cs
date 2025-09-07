using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] {"fileName"})]
        public IActionResult GetFile([FromQuery] string filename)
        {
            // sciezka bazowa do projektu
            var rootPath = Directory.GetCurrentDirectory();

            var filePath = $"{rootPath}/PrivateFiles/{filename}";

            var exist = System.IO.File.Exists(filePath);

            if (!exist)
            {
                return NotFound();
            }
            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out var contentType);

            var fileContents = System.IO.File.ReadAllBytes(filePath);

            /// Do zwracania plików jest metoda File
            /// Przyjmuje ona 3 paramtery plik jako tablice bajtów, typ pliku, nazwa pliku
            return File(fileContents, contentType , filename);
        }

        [HttpPost]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fullPath = $"{rootPath}/PrivateFiles/{file.FileName}";

                using(var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok();
            }

            return BadRequest();
        }
    }
}
