using System;

namespace Gi7.Model.Feed
{
    public class DeleteFeed : Feed
    {
        public String RefType { get; set; }
        public String Ref { get; set; }

        public override String Template
        {
            get
            {
                return String.Format("deleted branch {0} at", Ref);
            }
        }

        public override String Image
        {
            get
            {
                return "/Gi7;component/Images/delete.png";
            }
        }
    }
}
