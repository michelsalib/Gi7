using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private readonly GithubService githubService;
        private UserEventsRequests _eventsRequest;
        private UserFollowingRequest _followingsRequest;
        private bool? _isFollowing;
        private ObservableCollection<Repository> _repos;
        private bool _showAppBar;
        private User _user;
        private String _username;
        private UserFollowersRequest followersRequest;
        private RepositoriesRequest repositoriesRequest;
        private RepositoriesWatchedRequest repositoriesWatchedRequest;

        public UserViewModel(GithubService githubService, INavigationService navigationService, string user)
        {
            this.githubService = githubService;
            Username = user;
            EventsRequest = new UserEventsRequests(Username);
            ShowAppBar = false;

            ShareCommand = new RelayCommand(() => new ShareLinkTask
            {
                LinkUri = new Uri(User.HtmlUrl),
                Title = User.Name + " is on Github.",
                Message = "I found his profile on Github, you might want to see it.",
            }.Show(), () => User != null);
            FollowCommand = new RelayCommand(() => githubService.Load(new FollowUserRequest(Username, FollowUserRequest.Type.FOLLOW), r => { IsFollowing = true; }), () => IsFollowing.HasValue && !IsFollowing.Value);
            UnFollowCommand = new RelayCommand(() => githubService.Load(new FollowUserRequest(Username, FollowUserRequest.Type.UNFOLLOW), r => { IsFollowing = false; }), () => IsFollowing.HasValue && IsFollowing.Value);
            RepoSelectedCommand = new RelayCommand<Repository>(r => OnRepoSelected(navigationService, r));
            UserSelectedCommand = new RelayCommand<User>(u => OnUserSelected(navigationService, user, u));
            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnPivotChanged);
        }

        private static void OnRepoSelected(INavigationService navigationService, Repository r)
        {
            if (r != null)
                navigationService.NavigateTo(String.Format(ViewModelLocator.REPOSITORY_URL, r.Owner.Login, r.Name));
        }

        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand ProfileCommand { get; private set; }
        public RelayCommand FollowCommand { get; private set; }
        public RelayCommand UnFollowCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }

        public bool? IsFollowing
        {
            get { return _isFollowing; }
            set
            {
                if (value != _isFollowing)
                {
                    _isFollowing = value;
                    RaisePropertyChanged("IsFollowing");
                    FollowCommand.RaiseCanExecuteChanged();
                    UnFollowCommand.RaiseCanExecuteChanged();
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

        public String Username
        {
            get { return _username; }
            set
            {
                {
                    if (value != _username)
                    {
                        _username = value;
                        RaisePropertyChanged("Username");
                    }
                }
            }
        }

        public User User
        {
            get { return _user; }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged("User");
                    ShareCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public UserEventsRequests EventsRequest
        {
            get { return _eventsRequest; }
            set
            {
                if (_eventsRequest != value)
                {
                    _eventsRequest = value;
                    RaisePropertyChanged("EventsRequest");
                }
            }
        }

        public IEnumerable<Repository> OwnedRepos
        {
            get
            {
                if (Repos != null)
                    return Repos.Where(r => r.Owner.Login == Username);
                return null;
            }
        }

        public IEnumerable<Repository> WatchedRepos
        {
            get
            {
                if (Repos != null)
                    return Repos.Where(r => r.Owner.Login != Username);
                return null;
            }
        }

        public ObservableCollection<Repository> Repos
        {
            get { return _repos; }
            set
            {
                if (_repos != value)
                {
                    _repos = value;
                    RaisePropertyChanged("Repos");
                }
            }
        }

        public UserFollowingRequest FollowingsRequest
        {
            get { return _followingsRequest; }
            set
            {
                if (_followingsRequest != value)
                {
                    _followingsRequest = value;
                    RaisePropertyChanged("FollowingsRequest");
                }
            }
        }

        public UserFollowersRequest FollowersRequest
        {
            get { return followersRequest; }
            set
            {
                if (followersRequest != value)
                {
                    followersRequest = value;
                    RaisePropertyChanged("FollowersRequest");
                }
            }
        }

        public RepositoriesRequest RepositoriesRequest
        {
            get { return repositoriesRequest; }
            set
            {
                if (repositoriesRequest != value)
                {
                    repositoriesRequest = value;
                    RaisePropertyChanged("RepositoriesRequest");
                }
            }
        }

        public RepositoriesWatchedRequest RepositoriesWatchedRequest
        {
            get { return repositoriesWatchedRequest; }
            set
            {
                if (repositoriesWatchedRequest != value)
                {
                    repositoriesWatchedRequest = value;
                    RaisePropertyChanged("RepositoriesWatchedRequest");
                }
            }
        }

        private static void OnUserSelected(INavigationService navigationService, string user, User u)
        {
            if (user != null)
                navigationService.NavigateTo(string.Format(ViewModelLocator.USER_URL, u.Login));
        }

        private void OnPivotChanged(SelectionChangedEventArgs args)
        {
            var header = ((PivotItem)args.AddedItems[0]).Header as string;
            ShowAppBar = false;
            switch (header)
            {
                case "news":
                    if (EventsRequest == null)
                        EventsRequest = new UserEventsRequests(Username);
                    break;
                case "owned repos":
                    if (RepositoriesRequest == null)
                        RepositoriesRequest = new RepositoriesRequest(Username);
                    break;
                case "watched repos":
                    if (RepositoriesWatchedRequest == null)
                        RepositoriesWatchedRequest = new RepositoriesWatchedRequest(Username);
                    break;
                case "follower":
                    if (FollowersRequest == null)
                        FollowersRequest = new UserFollowersRequest(Username);
                    break;
                case "following":
                    if (FollowingsRequest == null)
                        FollowingsRequest = new UserFollowingRequest(Username);
                    break;
                case "details":
                    if (User == null)
                    {
                        User = githubService.Load(new UserRequest(Username), u => User = u);
                        if (githubService.IsAuthenticated)
                            IsFollowing = githubService.Load(new FollowUserRequest(Username), r => { IsFollowing = r; });
                    }
                    ShowAppBar = true;
                    break;
            }
        }
    }
}