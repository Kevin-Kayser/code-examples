using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace code_examples.Services
{
    public class FileSystemService : IFileSystemService
    {
        private AppSettings _appSettings { get; set; }

        public FileSystemService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string ProjectRoot => _appSettings.RootFolder;

        public FileDataAttributes GetFileFromPath(string fullFilePath)
        {
            if (File.Exists(fullFilePath))
            {
                FileAttributes attributes = File.GetAttributes(fullFilePath);
                var bytes = File.ReadAllBytes(fullFilePath);
                var provider = new FileExtensionContentTypeProvider();

                if (!provider.TryGetContentType(fullFilePath, out var contentType))
                {
                    contentType = "application/octet-stream";
                }

                return new FileDataAttributes
                {
                    Content = bytes,
                    Filename = Path.GetFileName(fullFilePath),
                    Type = contentType
                };
            }
            else
            {
                return null;
            }
        }

        public List<FolderContentResponse> GetFullFolderContents(string rootFolderPath,
            bool showFiles = false)
        {
            rootFolderPath ??= ProjectRoot;
            var returnData = new List<FolderContentResponse>();
            var rootDirectory = new DirectoryInfo(rootFolderPath);

            if (rootDirectory.Exists)
            {
                if (showFiles)
                {
                    // ignores folders and files with hidden attributes
                    var filesInThisFolder = rootDirectory.GetFiles().Select(f => f)
                        .Where(f => (f.Attributes & FileAttributes.Hidden) == 0).ToList().Select(x =>
                            new FolderContentResponse()
                            {
                                Label = x.Name,
                                IsFolder = false,
                                FullPath = x.FullName,
                                Leaf = true,
                                Selectable = true,
                            });


                    returnData.AddRange(filesInThisFolder);
                }
            }

            foreach (string d in Directory.GetDirectories(rootFolderPath))
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(d);

                    // ignores folders and files with hidden attributes
                    if ((directory.Attributes & FileAttributes.Hidden) == 0)
                    {
                        returnData.Add(new FolderContentResponse()
                        {
                            Label = directory.Name,
                            IsFolder = true,
                            FullPath = directory.FullName,
                            Leaf = false,
                            Children = null,
                            Selectable = false,
                        });
                    }
                }
                catch (Exception e)
                {
                    // Should catch and throw when appropriate. If there's any folder you don't have access to, an error may be thrown and ignored.
                }
            }

            return returnData;
        }
    }
}