using System.Collections.Generic;

namespace code_examples
{
    public class FolderContentResponse
    {
        public string Key => Label;
        public string Label { get; set; }

        public bool IsFolder { get; set; }
        public bool? FolderHasContents { get; set; }
        public string FullPath { get; set; }
        public bool Leaf { get; set; }
        public List<FolderContentResponse> Children { get; set; }
    }
}