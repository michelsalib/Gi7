using System;

namespace Gi7.Model.Feed
{
    public class CreateFeed : Feed
    {
        public String MasterBranch { get; set; }
        public String RefType { get; set; }
        public String Description { get; set; }
        public String Ref { get; set; }

        public override String Template
        {
            get
            {
                return String.Format("created branch {0} at", Ref);
            }
        }

        public override String Image
        {
            get
            {
                return "/Gi7;component/Images/create.png";
            }
        }
    }
}
