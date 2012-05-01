using System;

namespace Gi7.Client.Model
{
    public class GitHubFile : BoolModel
    {
        public String Type { get; set; }
        public String Url { get; set; }
        public int Size { get; set; }
        public String Path { get; set; }
        public String Sha { get; set; }
        public String Mode { get; set; }

        public override string ToString()
        {
            return string.Format("Type: {0} - Path: {1}", Type, Path);
        }
    }
}
