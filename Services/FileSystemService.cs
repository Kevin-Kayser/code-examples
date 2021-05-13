using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public List<FolderContentResponse> GetFullFolderContents(string rootFolderPath,
            bool showOnlyFolders = false)
        {
            rootFolderPath ??= ProjectRoot;
            var returnData = new List<FolderContentResponse>();
            var rootDirectory = new DirectoryInfo(rootFolderPath);

            if (rootDirectory.Exists)
            {
                if (!showOnlyFolders)
                {
                    var filesInThisFolder = rootDirectory.GetFiles().Select(f => f)
                        .Where(f => (f.Attributes & FileAttributes.Hidden) == 0).ToList().Select(x =>
                            new FolderContentResponse()
                            {
                                Label = x.Name,
                                IsFolder = false,
                                FullPath = x.FullName
                            });


                    returnData.AddRange(filesInThisFolder);
                }
            }

            foreach (string d in Directory.GetDirectories(rootFolderPath))
            {
                try
                {
                    DirectoryInfo directory = new DirectoryInfo(d);

                    if ((directory.Attributes & FileAttributes.Hidden) == 0)
                    {
                        returnData.Add(new FolderContentResponse()
                        {
                            Label = directory.Name,
                            IsFolder = true,
                            FullPath = directory.FullName,
                            Leaf = false,

                            FolderHasContents = directory.GetFiles().Any() || directory.GetDirectories().Any(),
                            Children = null
                        });
                    }
                }
                catch (Exception e)
                {
                    var x = 10;
                }
            }

            return returnData;
        }
    }
}