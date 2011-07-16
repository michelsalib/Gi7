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
using System.Collections.ObjectModel;

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

        private ObservableCollection<Push> _commits;
        public ObservableCollection<Push> Commits
        {
            get { return _commits; }
            set
            {
                if (_commits != value)
                {
                    _commits = value;
                    RaisePropertyChanged("Commits");
                }
            }
        }

        private ObservableCollection<PullRequest> _pullRequests;
        public ObservableCollection<PullRequest> PullRequests
        {
            get { return _pullRequests; }
            set
            {
                if (_pullRequests != value)
                {
                    _pullRequests = value;
                    RaisePropertyChanged("PullRequests");
                }
            }
        }

        public RepositoryViewModel(GithubService githubService, String user, String repo)
        {
            Repository = githubService.GetRepository(user, repo, r => Repository = r);

            Commits = githubService.GetCommits(user, repo);

            PullRequests = githubService.GetPullRequests(user, repo);
        }
    }
}
