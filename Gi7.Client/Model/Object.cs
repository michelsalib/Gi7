using System;

namespace Gi7.Client.Model
{
    public class Object : BoolModel
    {
        public String Type { get; set; }
        public String Url { get; set; }
        public int Size { get; set; }
        public String Path { get; set; }
        public String Sha { get; set; }
        public String Mode { get; set; }
    }
}
