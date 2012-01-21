﻿using System;

namespace Gi7.Model
{
    public class File
    {
        public String Status { get; set; }

        public String Patch { get; set; }

        public int Additions { get; set; }

        public String Filename { get; set; }

        public String Sha { get; set; }

        public String RawUrl { get; set; }

        public String BlobUrl { get; set; }

        public int Deletions { get; set; }

        public int Changes { get; set; }
    }
}