using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Client.Request;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class RepositoryViewModel : ViewModelBase
    {
        private Branch _branch;
        private ObservableCollection<Branch> _branches;
        private CommitListRequest _commitsRequest;
        private bool? _isWatching;
        private IssueListRequest _issuesRequest;
        private PullRequestListRequest _pullRequestsRequest;
        private Repository _repository;
        private bool _showAppBar;
        private GitTree _tree;
        private RepositoryCollaboratorsRequest collaboratorRequestRequest;
        private RepositoryWatchersRequest watchersRequestRequest;

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, String user, String repo)
        {
            ShowAppBar = true;

            Repository = githubService.Load(new RepositoryRequest(user, repo), r => Repository = r);

            if (githubService.IsAuthenticated)
                IsWatching = githubService.Load(new WatchRepositoryRequest(user, repo), r => { IsWatching = r; });

            Branches = githubService.Load(new RepositoryBranchesRequest(user, repo), b =>
            {
                Branches = b;
                Branch = b.FirstOrDefault(br => br.Name == "master");
            });

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Branch")
                {
                    CommitsRequest = null;
                    Tree = githubService.Load(new TreeRequest(user, repo, Branch.Commit.Sha), t => Tree = t);
                }
            };

            ObjectSelectedCommand = new RelayCommand<GitHubFile>(o => navigationService.NavigateTo(o.Type == "blob"
                                                                                                       ? String.Format(ViewModelLocator.BLOB_URL, user, repo, o.Sha, o.Path)
                                                                                                       : String.Format(ViewModelLocator.TREE_URL, user, repo, o.Sha, o.Path)));

            DownloadCommand = new RelayCommand(() => new WebBrowserTask
            {
                Uri = new Uri(Repository.HtmlUrl + "/zipball/" + Branch.Name),
            }.Show(), () => Repository != null && Branch != null);

            ShareDownloadCommand = new RelayCommand(() => new ShareLinkTask
            {
                LinkUri = new Uri(Repository.HtmlUrl + "/zipball/" + Branch.Name),
                Title = Repository.Fullname + " sources are on Github.",
                Message = "I found this sources on Github, you might want to get it.",
            }.Show(), () => Repository != null && Branch != null);

            ShareCommand = new RelayCommand(() => new ShareLinkTask
            {
                LinkUri = new Uri(Repository.HtmlUrl),
                Title = Repository.Fullname + " is on Github.",
                Message = "I found this repository on Github, you might want to see it.",
            }.Show(), () => Repository != null);

            NewIssueCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.CREATE_ISSUE_URL, user, repo)), () => githubService.IsAuthenticated);

            OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.USER_URL, Repository.Owner.Login)));

            WatchCommand = new RelayCommand(() => githubService.Load(new WatchRepositoryRequest(user, repo, WatchRepositoryRequest.Type.WATCH), r => { IsWatching = true; }), () => IsWatching.HasValue && !IsWatching.Value);

            UnWatchCommand = new RelayCommand(() => githubService.Load(new WatchRepositoryRequest(user, repo, WatchRepositoryRequest.Type.UNWATCH), r => { IsWatching = false; }), () => IsWatching.HasValue && IsWatching.Value);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args => OnPivotChanged(user, repo, args));
            CommitSelectedCommand = new RelayCommand<Push>(push =>
            {
                if (push)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.COMMIT_URL, Repository.Owner.Login, Repository.Name, push.Sha));
            });
            PullRequestSelectedCommand = new RelayCommand<PullRequest>(pullRequest =>
            {
                if (pullRequest)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.PULL_REQUEST_URL, Repository.Owner.Login, Repository.Name, pullRequest.Number));
            });
            IssueSelectedCommand = new RelayCommand<Issue>(issue =>
            {
                if (issue)
                {
                    var destination = issue.PullRequest.HtmlUrl == null ? ViewModelLocator.ISSUE_URL : ViewModelLocator.PULL_REQUEST_URL;
                    navigationService.NavigateTo(String.Format(destination, Repository.Owner.Login, Repository.Name, issue.Number));
                }
            });
            UserCommand = new RelayCommand<User>(collaborator => navigationService.NavigateTo(String.Format(ViewModelLocator.USER_URL, collaborator.Login)));
        }

        public RelayCommand ShareDownloadCommand { get; private set; }
        public RelayCommand DownloadCommand { get; private set; }
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand NewIssueCommand { get; private set; }
        public RelayCommand OwnerCommand { get; private set; }
        public RelayCommand WatchCommand { get; private set; }
        public RelayCommand UnWatchCommand { get; private set; }
        public RelayCommand<GitHubFile> ObjectSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand<User> UserCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }
        public RelayCommand<PullRequest> PullRequestSelectedCommand { get; private set; }
        public RelayCommand<Issue> IssueSelectedCommand { get; private set; }

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

        public CommitListRequest CommitsRequest
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

        public RepositoryCollaboratorsRequest CollaboratorRequestRequest
        {
            get { return collaboratorRequestRequest; }
            set
            {
                if (collaboratorRequestRequest != value)
                {
                    collaboratorRequestRequest = value;
                    RaisePropertyChanged("CollaboratorRequest");
                }
            }
        }

        public RepositoryWatchersRequest WatchersRequestRequest
        {
            get { return watchersRequestRequest; }
            set
            {
                if (watchersRequestRequest != value)
                {
                    watchersRequestRequest = value;
                    RaisePropertyChanged("WatchersRequest");
                }
            }
        }

        public PullRequestListRequest PullRequestsRequest
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

        public IssueListRequest IssuesRequest
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

        private void OnPivotChanged(string user, string repo, SelectionChangedEventArgs args)
        {
            var header = ((PivotItem) args.AddedItems[0]).Header as String;
            ShowAppBar = false;
            switch (header)
            {
            case "commits":
                if (CommitsRequest == null)
                    CommitsRequest = new CommitListRequest(user, repo, Branch ? Branch.Name : "master");
                break;
            case "pull requests":
                if (PullRequestsRequest == null)
                    PullRequestsRequest = new PullRequestListRequest(user, repo);
                break;
            case "issues":
                if (IssuesRequest == null)
                    IssuesRequest = new IssueListRequest(user, repo);
                ShowAppBar = true;
                break;
            case "collaborators":
                if (CollaboratorRequestRequest == null)
                    CollaboratorRequestRequest = new RepositoryCollaboratorsRequest(user, repo);
                break;
            case "watchers":
                if (WatchersRequestRequest == null)
                    WatchersRequestRequest = new RepositoryWatchersRequest(user, repo);
                break;
            case "details":
                ShowAppBar = true;
                break;
            }
        }
    }
}