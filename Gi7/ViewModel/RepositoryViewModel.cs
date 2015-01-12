using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class RepositoryViewModel : ViewModelBase
    {
        private readonly GithubService githubService;
        private readonly INavigationService navigationService;
        private Branch _branch;
        private ObservableCollection<Branch> _branches;
        private CommitListRequest _commitsRequest;
        private bool? _isWatching;
        private IssueListRequest _issuesRequest;
        private PullRequestListRequest _pullRequestsRequest;
        private Repository _repository;
        private bool _showAppBar;
        private GitTree _tree;
        private bool? canPin;
        private RepositoryCollaboratorsRequest collaboratorRequest;
        private RepositoryWatchersRequest watchersRequest;

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, string user, string repo)
        {
            this.githubService = githubService;
            this.navigationService = navigationService;
            Load(user, repo);
        }

        public void Load(string user, string repo)
        {
            if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(repo))
            {
                Loaded = true;
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
                                                                                                           ? string.Format(ViewModelLocator.BLOB_URL, user, repo, o.Sha, o.Path)
                                                                                                           : string.Format(ViewModelLocator.TREE_URL, user, repo, o.Sha, o.Path)));

                DownloadCommand = new RelayCommand(() => new WebBrowserTask
                {
                    Uri = new Uri(Repository.HtmlUrl + "/zipball/" + Branch.Name),
                }.Show(), () => Repository != null && Branch != null);

                ShareDownloadCommand = new RelayCommand(OnShareDownload, () => Repository != null && Branch != null);

                ShareCommand = new RelayCommand(OnShare, () => Repository != null);

                NewIssueCommand = new RelayCommand(() => navigationService.NavigateTo(string.Format(ViewModelLocator.CREATE_ISSUE_URL, user, repo)), () => githubService.IsAuthenticated);

                OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(string.Format(ViewModelLocator.USER_URL, Repository.Owner.Login)));

                WatchCommand = new RelayCommand(() => githubService.Load(new WatchRepositoryRequest(user, repo, WatchRepositoryRequest.Type.WATCH), r => { IsWatching = true; }), () => IsWatching.HasValue && !IsWatching.Value);

                UnWatchCommand = new RelayCommand(() => githubService.Load(new WatchRepositoryRequest(user, repo, WatchRepositoryRequest.Type.UNWATCH), r => { IsWatching = false; }), () => IsWatching.HasValue && IsWatching.Value);

                PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args => OnPivotChanged(user, repo, args));
                CommitSelectedCommand = new RelayCommand<Push>(OnCommitSelected);
                PullRequestSelectedCommand = new RelayCommand<PullRequest>(OnPullRequest);
                IssueSelectedCommand = new RelayCommand<Issue>(OnIssueSelected);
                UserCommand = new RelayCommand<User>(collaborator => navigationService.NavigateTo(string.Format(ViewModelLocator.USER_URL, collaborator.Login)));
                PinCommand = new RelayCommand(() => OnPin(user, repo), () => CanPin == true);

                CanPin = !ShellTile.ActiveTiles.Any(t => IsPinned(user, repo, t));
            }
        }

        public bool Loaded { get; set; }

        public RelayCommand ShareDownloadCommand { get; private set; }
        public RelayCommand DownloadCommand { get; private set; }
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand NewIssueCommand { get; private set; }
        public RelayCommand OwnerCommand { get; private set; }
        public RelayCommand WatchCommand { get; private set; }
        public RelayCommand UnWatchCommand { get; private set; }
        public RelayCommand PinCommand { get; private set; }
        public RelayCommand<GitHubFile> ObjectSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand<User> UserCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }
        public RelayCommand<PullRequest> PullRequestSelectedCommand { get; private set; }
        public RelayCommand<Issue> IssueSelectedCommand { get; private set; }

        public bool? CanPin
        {
            get { return canPin; }
            set
            {
                if (canPin != value)
                {
                    canPin = value;
                    RaisePropertyChanged("CanPin");
                    PinCommand.RaiseCanExecuteChanged();
                }
            }
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

        public RepositoryCollaboratorsRequest CollaboratorRequest
        {
            get { return collaboratorRequest; }
            set
            {
                if (collaboratorRequest != value)
                {
                    collaboratorRequest = value;
                    RaisePropertyChanged("CollaboratorRequest");
                }
            }
        }

        public RepositoryWatchersRequest WatchersRequest
        {
            get { return watchersRequest; }
            set
            {
                if (watchersRequest != value)
                {
                    watchersRequest = value;
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

        private static bool IsPinned(string user, string repo, ShellTile t)
        {
            return t.NavigationUri.Equals(new Uri(string.Format(ViewModelLocator.REPOSITORY_URL, user, repo), UriKind.RelativeOrAbsolute));
        }

        private void OnPin(string user, string repo)
        {
            var isoStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isoStorageFile.DirectoryExists("Shared\\ShellContent"))
            {
                isoStorageFile.CreateDirectory("Shared\\ShellContent");
                using (var to = isoStorageFile.OpenFile("Shared\\ShellContent\\Background.png", FileMode.Create))
                {
                    var from = Application.GetResourceStream(new Uri("Background.png", UriKind.Relative));
                    from.Stream.CopyTo(to);
                }
            }
            var uri = new Uri(string.Format(ViewModelLocator.REPOSITORY_URL, user, repo), UriKind.Relative);
            CanPin = false;
            ShellTileData tile = new StandardTileData { Title = Repository.Name, BackgroundImage = new Uri("isostore:/Shared/ShellContent/Background.png") };
            ShellTile.Create(uri, tile);
        }

        private void OnIssueSelected(Issue issue)
        {
            if (issue)
            {
                var destination = (issue.PullRequest == null || issue.PullRequest.HtmlUrl == null)
                    ? ViewModelLocator.ISSUE_URL
                    : ViewModelLocator.PULL_REQUEST_URL;
                navigationService.NavigateTo(string.Format(destination, Repository.Owner.Login, Repository.Name, issue.Number));
            }
        }

        private void OnPullRequest(PullRequest pullRequest)
        {
            if (pullRequest)
                navigationService.NavigateTo(string.Format(ViewModelLocator.PULL_REQUEST_URL, Repository.Owner.Login, Repository.Name, pullRequest.Number));
        }

        private void OnCommitSelected(Push push)
        {
            if (push)
                navigationService.NavigateTo(string.Format(ViewModelLocator.COMMIT_URL, Repository.Owner.Login, Repository.Name, push.Sha));
        }

        private void OnShare()
        {
            new ShareLinkTask
            {
                LinkUri = new Uri(Repository.HtmlUrl),
                Title = Repository.Fullname + " is on Github.",
                Message = "I found this repository on Github, you might want to see it.",
            }.Show();
        }

        private void OnShareDownload()
        {
            new ShareLinkTask
            {
                LinkUri = new Uri(Repository.HtmlUrl + "/zipball/" + Branch.Name),
                Title = Repository.Fullname + " sources are on Github.",
                Message = "I found this sources on Github, you might want to get it.",
            }.Show();
        }

        private void OnPivotChanged(string user, string repo, SelectionChangedEventArgs args)
        {
            var header = ((PivotItem) args.AddedItems[0]).Header as string;
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
                if (CollaboratorRequest == null)
                    CollaboratorRequest = new RepositoryCollaboratorsRequest(user, repo);
                break;
            case "watchers":
                if (WatchersRequest == null)
                    WatchersRequest = new RepositoryWatchersRequest(user, repo);
                break;
            case "details":
                ShowAppBar = true;
                break;
            }
        }
    }
}