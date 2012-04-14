using System;

namespace Gi7.Client.Model
{
    public class Blob : BoolModel
    {
        public String Content { get; set; }
        public String Url { get; set; }
        public int Size { get; set; }
        public String Encoding { get; set; }
        public String Sha { get; set; }
    }
}
