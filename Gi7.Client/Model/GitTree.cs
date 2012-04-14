using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gi7.Client.Model
{
    public class GitTree : BoolModel
    {
        public String Sha { get; set; }
        public String Url { get; set; }
        public List<Object> Tree { get; set; }
    }
}
