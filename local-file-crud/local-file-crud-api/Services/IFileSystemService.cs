using System.Collections.Generic;

namespace code_examples.Services
{
    public interface IFileSystemService
    {
        string ProjectRoot { get; }
        List<FolderContentResponse> GetFullFolderContents(string rootFolderPath, bool showFiles = false);
        FileDataAttributes GetFileFromPath(string fullFilePath);
    }
}