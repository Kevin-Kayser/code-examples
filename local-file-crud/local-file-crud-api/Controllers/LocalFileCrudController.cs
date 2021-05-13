using System.Threading.Tasks;
using code_examples.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace code_examples.Controllers
{
    /// <summary>
    /// Local File Crud will accept directions from the UI and will load local resource data.
    /// Cors is explicitly for known UI server in Startup.cs. This will need to change when deployed to real server/service.
    /// </summary>
    [EnableCors("CorsPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class LocalFileCrudController : ControllerBase
    {
        private readonly IFileSystemService _fileSystemService;

        public LocalFileCrudController(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> FolderContents(string innerFolder = null)
        {
            return Ok(_fileSystemService.GetFullFolderContents(innerFolder, true));
        }
    }
}