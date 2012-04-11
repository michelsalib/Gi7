using System;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Service;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Gi7.Client;

namespace Gi7.Views
{
    public class RepositoryViewModel : ViewModelBase
    {
        private bool _showAppBar;
        private bool? _isWatching;
        private Branch _branch;
        private BranchesRequest _branchesRequest;
        private CollaboratorRequest _collaboratorRequest;
        private WatchersRequest _watchersRequest;
        private CommitsRequest _commitsRequest;
        private IssuesRequest _issuesRequest;
        private PullRequestsRequest _pullRequestsRequest;
        private Repository _repository;

        public RelayCommand OwnerCommand { get; private set; }
        public RelayCommand WatchCommand { get; private set; }
        public RelayCommand UnWatchCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand<ListPicker> BranchChangedCommand { get; private set; }
        public RelayCommand<User> UserCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }
        public RelayCommand<PullRequest> PullRequestSelectedCommand { get; private set; }
        public RelayCommand<Issue> IssueSelectedCommand { get; private set; }

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, String user, String repo)
        {
            ShowAppBar = false;
            
            Repository = githubService.Load(new RepositoryRequest(user, repo), r => Repository = r);
            IsWatching = githubService.Load(new RepositoryWatchRequest(user, repo), r =>
            {
                IsWatching = r;
            });

            OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, Repository.Owner.Login)));

            if (BranchesRequest == null)
            {
                BranchesRequest = new BranchesRequest(user, repo);
                BranchesRequest.Success += (s, e) =>
                {
                    Branch = e.NewResult.FirstOrDefault(b => b.Name == "master");
                };
            }

            BranchChangedCommand = new RelayCommand<ListPicker>(e =>
            {
                if (e != null)
                {
                    CommitsRequest = null;
                }
            });

            WatchCommand = new RelayCommand(() =>
            {
                githubService.Load(new RepositoryWatchRequest(user, repo, RepositoryWatchRequest.Type.WATCH), r =>
                {
                    IsWatching = true;
                });
            }, () => IsWatching.HasValue && !IsWatching.Value);

            UnWatchCommand = new RelayCommand(() =>
            {
                githubService.Load(new RepositoryWatchRequest(user, repo, RepositoryWatchRequest.Type.UNWATCH), r =>
                {
                    IsWatching = false;
                });
            }, () => IsWatching.HasValue && IsWatching.Value);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = ((PivotItem)args.AddedItems[0]).Header as String;
                ShowAppBar = false;
                switch (header)
                {
                    case "Commits":
                        if (CommitsRequest == null)
                            CommitsRequest = new CommitsRequest(user, repo, Branch ? Branch.Name : "master");
                        break;
                    case "Pull requests":
                        if (PullRequestsRequest == null)
                            PullRequestsRequest = new PullRequestsRequest(user, repo);
                        break;
                    case "Issues":
                        if (IssuesRequest == null)
                            IssuesRequest = new IssuesRequest(user, repo);
                        break;
                    case "Collaborators":
                        if (CollaboratorRequest == null)
                            CollaboratorRequest = new CollaboratorRequest(user, repo);
                        break;
                    case "Watchers":
                        if (WatchersRequest == null)
                            WatchersRequest = new WatchersRequest(user, repo);
                        break;
                    case "Details":
                        ShowAppBar = false;
                        break;
                }
            });
            CommitSelectedCommand = new RelayCommand<Push>(push =>
            {
                if (push)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, push.Sha));
            });
            PullRequestSelectedCommand = new RelayCommand<PullRequest>(pullRequest =>
            {
                if (pullRequest)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.PullRequestUrl, Repository.Owner.Login, Repository.Name, pullRequest.Number));
            });
            IssueSelectedCommand = new RelayCommand<Issue>(issue =>
            {
                if (issue)
                {
                    string destination = issue.PullRequest.HtmlUrl == null ? ViewModelLocator.IssueUrl : ViewModelLocator.PullRequestUrl;
                    navigationService.NavigateTo(String.Format(destination, Repository.Owner.Login, Repository.Name, issue.Number));
                }
            });
            UserCommand = new RelayCommand<User>(collaborator => navigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, collaborator.Login)));
        }

        public bool? IsWatching
        {
            get { return _isWatching; }
            set
            {
                if (value != _isWatching)
                {
                    _isWatching = value;
                    RaisePropertyChanged("IsWatching");
                    WatchCommand.RaiseCanExecuteChanged();
                    UnWatchCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool ShowAppBar
        {

            get { return _showAppBar; }
            set
            {
                if (value != _showAppBar)
                {
                    _showAppBar = value;
                    RaisePropertyChanged("ShowAppBar");
                }
            }
        }

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

        public CommitsRequest CommitsRequest
        {
            get { return _commitsRequest; }
            set
            {
                if (_commitsRequest != value)
                {
                    _commitsRequest = value;
                    RaisePropertyChanged("CommitsRequest");
                }
            }
        }

        public BranchesRequest BranchesRequest
        {
            get { return _branchesRequest; }
            set
            {
                if (_branchesRequest != value)
                {
                    _branchesRequest = value;
                    RaisePropertyChanged("BranchesRequest");
                }
            }
        }

        public CollaboratorRequest CollaboratorRequest
        {
            get { return _collaboratorRequest; }
            set
            {
                if (_collaboratorRequest != value)
                {
                    _collaboratorRequest = value;
                    RaisePropertyChanged("CollaboratorRequest");
                }
            }
        }

        public WatchersRequest WatchersRequest
        {
            get { return _watchersRequest; }
            set
            {
                if (_watchersRequest != value)
                {
                    _watchersRequest = value;
                    RaisePropertyChanged("WatchersRequest");
                }
            }
        }

        public PullRequestsRequest PullRequestsRequest
        {
            get { return _pullRequestsRequest; }
            set
            {
                if (_pullRequestsRequest != value)
                {
                    _pullRequestsRequest = value;
                    RaisePropertyChanged("PullRequestsRequest");
                }
            }
        }

        public IssuesRequest IssuesRequest
        {
            get { return _issuesRequest; }
            set
            {
                if (_issuesRequest != value)
                {
                    _issuesRequest = value;
                    RaisePropertyChanged("IssuesRequest");
                }
            }
        }

        public Branch Branch
        {
            get { return _branch; }
            set
            {
                if (_branch != value)
                {
                    _branch = value;
                    RaisePropertyChanged("Branch");
                }
            }
        }
    }
}