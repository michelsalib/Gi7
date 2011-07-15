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
using GalaSoft.MvvmLight;
using Github7.Service;
using Github7.Model;

namespace Github7.Views
{
    public class RepositoryViewModel : ViewModelBase
    {
        private Repository _repository;
        public Repository Repository
        {
            get { return _repository; }
            set
            {
                if (_repository != value)
                {
                    _repository = value;
                    RaisePropertyChanged("Repository");
                }
            }
        }

        public RepositoryViewModel(GithubService githubService, String user, String repo)
        {
            Repository = githubService.GetRepository(user, repo, r => Repository = r);
        }
    }
}
