using System;
using GalaSoft.MvvmLight;
using Github7.Service;

namespace Github7.Views
{
    public class UserViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged("IsLoading");
                }
            }
        }

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

            // listening to loading
            githubService.Loading += (s, e) => IsLoading = e.IsLoading;
            IsLoading = githubService.IsLoading;
        }
    }
}
