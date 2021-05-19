using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace code_examples.Services
{
    public interface IEmailService
    {
        Task SendEmail(List<string> attachementFileList);
    }

    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;
        private readonly IFileSystemService _fileSystemService;

        public EmailService(IOptions<AppSettings> appSettings, IFileSystemService fileSystemService)
        {
            _appSettings = appSettings.Value;
            _fileSystemService = fileSystemService;
        }

        public async Task SendEmail(List<string> attachmentFileList)
        {
            var apiKey = _appSettings.SendGridApiKey;


            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("kevin@kayserlabs.com", "Kevin Kayser");
            var subject = $"Test email with attachments";

            var to = new EmailAddress("kevin.kayser@irwinseating.com", "Kevin Kayser");
            var plainTextContent = "Plain Text Content";
            var htmlContent = $"HTML Content";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            foreach (var filePath in attachmentFileList)
            {
                var fileData = _fileSystemService.GetFileFromPath(filePath);

                msg.AddAttachment(new SendGrid.Helpers.Mail.Attachment
                    {
                        Content = Convert.ToBase64String(fileData.Content),
                        Filename = fileData.Filename,
                        Type = fileData.Type,
                        Disposition = "attachment"
                    }
                );
            }


            var response = await client.SendEmailAsync(msg);
            return;
        }
    }
}