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
    public class RepositoryDataModel
    {
        public Repository Repository { get; set; }

        public RepositoryDataModel()
        {
            Repository = new Repository()
            {
                Owner = new User()
                {
                    Login = "michelsalib"
                },
                Name = "symfony",
                HtmlUrl = "http://github.com/michelsalib/symfony",
                Description = "The Symfony2 PHP framework",
                Watchers = 3,
                Forks = 1,
                Homepage = "symfony.com",
                Parent = new Repository()
                {
                    Owner = new User()
                    {
                        Login = "symfony"
                    },
                    Name = "symfnoy"
                }
            };
        }
    }
}
