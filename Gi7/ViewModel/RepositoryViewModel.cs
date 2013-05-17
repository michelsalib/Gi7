using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request.Repository;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class RepositoryViewModel : ViewModelBase
    {
        private bool _showAppBar;
        private bool? _isWatching;
        private GitTree _tree;
        private Branch _branch;
        private ObservableCollection<Branch> _branches;
        private Client.Request.Repository.ListCollaborators _collaboratorRequest;
        private Client.Request.Repository.ListWatchers _watchersRequest;
        private Client.Request.Commit.List _commitsRequest;
        private Client.Request.Issue.List _issuesRequest;
        private Client.Request.PullRequest.List _pullRequestsRequest;
        private Repository _repository;

        public RelayCommand ShareDownloadCommand { get; private set; }
        public RelayCommand DownloadCommand { get; private set; }
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand NewIssueCommand { get; private set; }
        public RelayCommand OwnerCommand { get; private set; }
        public RelayCommand WatchCommand { get; private set; }
        public RelayCommand UnWatchCommand { get; private set; }
        public RelayCommand<Gi7.Client.Model.GitHubFile> ObjectSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand<User> UserCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }
        public RelayCommand<PullRequest> PullRequestSelectedCommand { get; private set; }
        public RelayCommand<Issue> IssueSelectedCommand { get; private set; }

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, String user, String repo)
        {
            ShowAppBar = true;

            Repository = githubService.Load(new Client.Request.Repository.Get(user, repo), r => Repository = r);

            if (githubService.IsAuthenticated)
            {
                IsWatching = githubService.Load(new Watch(user, repo), r =>
                {
                    IsWatching = r;
                });
            }

            Branches = githubService.Load(new Client.Request.Repository.ListBranches(user, repo), b =>
            {
                Branches = b;
                Branch = b.FirstOrDefault(br => br.Name == "master");
            });

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Branch")
                {
                    CommitsRequest = null;
                    Tree = githubService.Load(new Client.Request.Tree.Get(user, repo, Branch.Commit.Sha), t => Tree = t);
                }
            };

            ObjectSelectedCommand = new RelayCommand<Client.Model.GitHubFile>(o =>
            {
                if (o.Type == "blob") {
                    navigationService.NavigateTo(String.Format(Service.ViewModelLocator.BlobUrl, user, repo, o.Sha, o.Path));
                }
                else { //tree
                    navigationService.NavigateTo(String.Format(Service.ViewModelLocator.TreeUrl, user, repo, o.Sha, o.Path));
                }
            });

            DownloadCommand = new RelayCommand(() =>
            {
                new WebBrowserTask()
                {
                    Uri = new Uri(Repository.HtmlUrl + "/zipball/" + Branch.Name),
                }.Show();
            }, () => Repository != null && Branch != null);

            ShareDownloadCommand = new RelayCommand(() =>
            {
                new ShareLinkTask()
                {
                    LinkUri = new Uri(Repository.HtmlUrl + "/zipball/" + Branch.Name),
                    Title = Repository.Fullname + " sources are on Github.",
                    Message = "I found this sources on Github, you might want to get it.",
                }.Show();
            }, () => Repository != null && Branch != null);

            ShareCommand = new RelayCommand(() =>
            {
                new ShareLinkTask()
                {
                    LinkUri = new Uri(Repository.HtmlUrl),
                    Title = Repository.Fullname + " is on Github.",
                    Message = "I found this repository on Github, you might want to see it.",
                }.Show();
            }, () => Repository != null);

            NewIssueCommand = new RelayCommand(() =>
            {
                navigationService.NavigateTo(String.Format(Service.ViewModelLocator.CreateIssueUrl, user, repo));
            }, () => githubService.IsAuthenticated);

            OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(Service.ViewModelLocator.UserUrl, Repository.Owner.Login)));

            WatchCommand = new RelayCommand(() =>
            {
                githubService.Load(new Watch(user, repo, Watch.Type.WATCH), r =>
                {
                    IsWatching = true;
                });
            }, () => IsWatching.HasValue && !IsWatching.Value);

            UnWatchCommand = new RelayCommand(() =>
            {
                githubService.Load(new Watch(user, repo, Watch.Type.UNWATCH), r =>
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
                            CommitsRequest = new Client.Request.Commit.List(user, repo, Branch ? Branch.Name : "master");
                        break;
                    case "Pull requests":
                        if (PullRequestsRequest == null)
                            PullRequestsRequest = new Client.Request.PullRequest.List(user, repo);
                        break;
                    case "Issues":
                        if (IssuesRequest == null)
                            IssuesRequest = new Client.Request.Issue.List(user, repo);
                        ShowAppBar = true;
                        break;
                    case "Collaborators":
                        if (CollaboratorRequest == null)
                            CollaboratorRequest = new Client.Request.Repository.ListCollaborators(user, repo);
                        break;
                    case "Watchers":
                        if (WatchersRequest == null)
                            WatchersRequest = new Client.Request.Repository.ListWatchers(user, repo);
                        break;
                    case "Details":
                        ShowAppBar = true;
                        break;
                }
            });
            CommitSelectedCommand = new RelayCommand<Push>(push =>
            {
                if (push)
                    navigationService.NavigateTo(String.Format(Service.ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, push.Sha));
            });
            PullRequestSelectedCommand = new RelayCommand<PullRequest>(pullRequest =>
            {
                if (pullRequest)
                    navigationService.NavigateTo(String.Format(Service.ViewModelLocator.PullRequestUrl, Repository.Owner.Login, Repository.Name, pullRequest.Number));
            });
            IssueSelectedCommand = new RelayCommand<Issue>(issue =>
            {
                if (issue)
                {
                    string destination = issue.PullRequest.HtmlUrl == null ? Service.ViewModelLocator.IssueUrl : Service.ViewModelLocator.PullRequestUrl;
                    navigationService.NavigateTo(String.Format(destination, Repository.Owner.Login, Repository.Name, issue.Number));
                }
            });
            UserCommand = new RelayCommand<User>(collaborator => navigationService.NavigateTo(String.Format(Service.ViewModelLocator.UserUrl, collaborator.Login)));
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
                    ShareCommand.RaiseCanExecuteChanged();
                    ShareDownloadCommand.RaiseCanExecuteChanged();
                    DownloadCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public Client.Request.Commit.List CommitsRequest
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

        public ObservableCollection<Branch> Branches
        {
            get { return _branches; }
            set
            {
                if (_branches != value)
                {
                    _branches = value;
                    RaisePropertyChanged("Branches");
                }
            }
        }

        public Client.Request.Repository.ListCollaborators CollaboratorRequest
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

        public Client.Request.Repository.ListWatchers WatchersRequest
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

        public Client.Request.PullRequest.List PullRequestsRequest
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

        public Client.Request.Issue.List IssuesRequest
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

        public GitTree Tree
        {
            get { return _tree; }
            set
            {
                if (_tree != value)
                {
                    _tree = value;
                    RaisePropertyChanged("Tree");
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
                    
                    ShareDownloadCommand.RaiseCanExecuteChanged();
                    DownloadCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}