using System;

namespace Gi7.Client.Model
{
    public class Download : BoolModel
    {
        public String Url { get; set; }

        public String HtmlUrl { get; set; }

        public int Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public int Size { get; set; }

        public int DownloadCount { get; set; }

        public String ContentType { get; set; }
    }
}
