using System;

namespace Gi7.Model
{
    public class Committer : BoolModel
    {
        public DateTime Date { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }
    }
}