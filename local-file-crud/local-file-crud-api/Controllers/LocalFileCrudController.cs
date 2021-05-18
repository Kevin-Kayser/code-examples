using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using code_examples.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

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
        private readonly IEmailService _emailService;

        public LocalFileCrudController(IFileSystemService fileSystemService, IEmailService emailService)
        {
            _fileSystemService = fileSystemService;
            _emailService = emailService;
        }

        [HttpGet("")]
        public async Task<IActionResult> FolderContents(string innerFolder = null, bool showFiles = false)
        {
            return Ok(_fileSystemService.GetFullFolderContents(innerFolder, showFiles));
        }

        [HttpPost("sendFiles")]
        public async Task<IActionResult> sendFiles(List<string> filesToSend)
        {
            _emailService.SendEmail(filesToSend);

            return Ok(filesToSend);
        }
    }
}