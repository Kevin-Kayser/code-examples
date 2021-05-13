using System.Threading.Tasks;
using code_examples.Services;
using Microsoft.AspNetCore.Mvc;

namespace code_examples.Controllers
{
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