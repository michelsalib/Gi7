using System;

namespace Gi7.Model.Extra
{
    public class FeaturedRepo
    {
        public String Title { get; set; }

        public String User { get; set; }

        public String Repo { get; set; }

        public String Fullname
        {
            get { return String.Format("{0}/{1}", User, Repo); }
        }
    }
}