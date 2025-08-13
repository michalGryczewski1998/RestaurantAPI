using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Information;
using System.Runtime.InteropServices;

namespace RestaurantAPI.Controllers
{
    [Route("api/info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet("")]
        public ActionResult AppInfo()
        {
            var sysInfo = new SysInfo()
            {
                MachineName = Environment.MachineName, // Nazwa komputera/serwera
                OSDescription = RuntimeInformation.OSDescription, // Opis systemu
                OSArchitecture = RuntimeInformation.OSArchitecture.ToString(), // Architektura
                ProcessorCount = Environment.ProcessorCount, // Liczba rdzeni CPU
                Framework = RuntimeInformation.FrameworkDescription, // Wersja .NET
                CurrentDirectory = Environment.CurrentDirectory, // Folder roboczy
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64) // Czas od uruchomienia systemu
            };

            return Ok(sysInfo);
        }
    }
}
