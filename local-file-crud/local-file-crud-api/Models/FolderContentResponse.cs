using System.Collections.Generic;

namespace code_examples
{
    public class FileDataAttributes
    {
        public string Content { get; set; }
        public string Filename { get; set; }
        public string Type { get; set; }
    }

    public class FolderContentResponse
    {
        public string Key => Label;
        public string Label { get; set; }

        public bool IsFolder { get; set; }
        public string FullPath { get; set; }
        public string Data => FullPath;
        public bool Leaf { get; set; }
        public bool Selectable { get; set; }
        public List<FolderContentResponse> Children { get; set; }
    }
}