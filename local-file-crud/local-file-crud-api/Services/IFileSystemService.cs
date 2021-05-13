using System.Collections.Generic;

namespace code_examples.Services
{
    public interface IFileSystemService
    {
        List<FolderContentResponse> GetFullFolderContents(string rootFolderPath, bool showOnlyFolders = false);
        string ProjectRoot { get; }
    }
}