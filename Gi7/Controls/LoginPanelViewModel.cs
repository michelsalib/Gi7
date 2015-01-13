using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using System;

namespace Gi7.Controls
{
    public class LoginPanelViewModel : ViewModelBase
    {
        private String _email;

        private String _password;

        private readonly GithubService _githubService;

        public LoginPanelViewModel(GithubService githubService)
        {
            _githubService = githubService;
            LoginCommand = new RelayCommand(LogIn);
        }

        private void LogIn()
        {
            _githubService.AuthenticateUser(Email, Password);
        }

        public String Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged();
                }
            }
        }

        public String Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand LoginCommand { get; private set; }
    }
}