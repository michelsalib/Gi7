using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gi7.Client.Model.Extra
{
    public class SearchedRepository
    {
        public String Fullname { get { return Username + "/" + Name; } }
        public String Name { get; set; }
        public String Username { get; set; }
    }
}
