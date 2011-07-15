using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Github7.Model;

namespace Github7.Resources.DesignData
{
    public class UserDataModel
    {
        public User User { get; set; }

        public UserDataModel()
        {
            User = new User()
            {
                Login = "michelsalib",
            };
        }
    }
}
