﻿using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Service;

namespace Gi7.Controls
{
    public class LoginPanelViewModel : ViewModelBase
    {
        private String _email;
        public String Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }

        private String _password;
        public String Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        public RelayCommand LoginCommand { get; private set; }

        public LoginPanelViewModel(GithubService githubService)
        {
            LoginCommand = new RelayCommand(() =>
            {
                githubService.AuthenticateUser(Email, Password);
            });
        }
    }
}
