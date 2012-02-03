using System;
using System.Linq;

namespace Gi7.Model
{
    public class User : BoolModel
    {
        private String _avatarUrl;
        public String Login { get; set; }

        public String Name { get; set; }

        public int Followers { get; set; }

        public int Following { get; set; }

        public String Location { get; set; }

        public String Email { get; set; }

        public String Company { get; set; }

        public String Blog { get; set; }

        public String Bio { get; set; }

        public String HtmlUrl { get; set; }

        public String Url { get; set; }

        public int PublicRepos { get; set; }

        public int TotalPrivateRepos { get; set; }

        public String AvatarUrl
        {
            get { return _avatarUrl; }
            set
            {
                // trim GET parameters
                _avatarUrl = new String(value.TakeWhile(c => c != '?').ToArray());
            }
        }
    }
}