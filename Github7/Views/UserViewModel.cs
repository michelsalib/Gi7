using System;
using GalaSoft.MvvmLight;
using Github7.Service;

namespace Github7.Views
{
    public class UserViewModel : ViewModelBase
    {
        private String _user;
        public String User
        {
            get { return _user; }
            set
            {
                {
                    if (value != _user)
                    {
                        _user = value;
                        RaisePropertyChanged("User");
                    }
                }
            }
        }
        

        public UserViewModel(GithubService githubService, string user)
        {
            User = user;
        }
    }
}
