using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Kawagoe.Storage;

namespace Github7.Model
{
    public class User
    {
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

        private String _avatarUrl;
        public String AvatarUrl
        {
            get
            {
                return _avatarUrl;
            }
            set
            {
                // trim GET parameters
                _avatarUrl = new String(value.TakeWhile(c => c != '?').ToArray()); 
            }
        }

        private ImageCache _cache = new PersistentImageCache("User");
        public ImageSource Avatar
        {
            get
            {
                return _cache.Get(AvatarUrl);
            }
        }
    }
}
