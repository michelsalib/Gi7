using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model;
using Github7.Service;
using Github7.Service.Navigation;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace Github7.Views
{
    public class RepositoryViewModel : ViewModelBase
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

        private ObservableCollection<Issue> _issues;
        public ObservableCollection<Issue> Issues
        {
            get { return _issues; }
            set
            {
                if (_issues != value)
                {
                    _issues = value;
                    RaisePropertyChanged("Issues");
                }
            }
        }

        public RelayCommand OwnerCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, String user, String repo)
        {
            Repository = githubService.GetRepository(user, repo, r => Repository = r);

            PullRequests = githubService.GetPullRequests(user, repo);

            Issues = githubService.GetIssues(user, repo);

            OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, Repository.Owner.Login)));
            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                    case "Commits":
                        if(Commits == null)
                            Commits = githubService.GetCommits(user, repo);
                        break;
                    default:
                        break;
                }
            });
            CommitSelectedCommand = new RelayCommand<Push>(push =>
            {
                if (push != null)
                {
                    navigationService.NavigateTo(String.Format(ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, push.Sha));
                }
            });

            // listening to loading
            githubService.Loading += (s, e) => IsLoading = e.IsLoading;
            IsLoading = githubService.IsLoading;
        }
    }
}
