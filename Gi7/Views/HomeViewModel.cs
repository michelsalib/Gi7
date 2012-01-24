using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Gi7.Model;
using Gi7.Model.Extra;
using Gi7.Model.Feed.Base;
using Gi7.Service;
using Gi7.Service.Navigation;
using Gi7.Service.Request;
using Gi7.Service.Request.Base;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly GithubService _githubService;
        private ObservableCollection<FeaturedRepo> _featuredRepos;
        private PrivateFeedsRequest _feedsRequest;
        private FollowersRequest _followersRequest;
        private FollowingsRequest _followingsRequest;
        private bool _isLoggedIn;
        private ObservableCollection<Repository> _repos;
        private ObservableCollection<Repository> _ownedRepos;
        private ObservableCollection<Repository> _watchedRepos;
        private User _user;

        public HomeViewModel(GithubService githubService, INavigationService navigationService)
        {
            _githubService = githubService;

            // commands
            FeaturedRepoSelectedCommand = new RelayCommand<FeaturedRepo>(r =>
            {
                if (r != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, r.User, r.Repo));
            });
            RepoSelectedCommand = new RelayCommand<Repository>(r =>
            {
                if (r != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, r.Owner.Login, r.Name));
            });
            FeedSelectedCommand = new RelayCommand<Feed>(feed =>
            {
                if (feed != null)
                    navigationService.NavigateTo(feed.Destination);
            });
            UserSelectedCommand = new RelayCommand<User>(user =>
            {
                if (user != null)
                    navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, user.Login));
            });
            PanoramaChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args => { _loadPanel((args.AddedItems[0] as PanoramaItem).Header as String); });

            // init
            if (_githubService.IsAuthenticated)
            {
                _login();
            } else
            {
                _logout();
            }

            // listenning to the github service
            githubService.IsAuthenticatedChanged += (s, e) =>
            {
                if (e.IsAuthenticated)
                {
                    _login();
                } else
                {
                    _logout();
                }
            };

            // listening to view events
            Messenger.Default.Register<bool>(this, "logout", b => githubService.Logout());
            Messenger.Default.Register<bool>(this, "about", b => navigationService.NavigateTo(ViewModelLocator.AboutUrl));
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    RaisePropertyChanged("IsLoggedIn");
                    RaisePropertyChanged("IsLoggedOut");
                }
            }
        }

        public bool IsLoggedOut
        {
            get { return !IsLoggedIn; }
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
                }
            }
        }

        public PrivateFeedsRequest FeedsRequest
        {
            get { return _feedsRequest; }
            set
            {
                if (_feedsRequest != value)
                {
                    _feedsRequest = value;
                    RaisePropertyChanged("FeedsRequest");
                }
            }
        }

        public IEnumerable<Repository> OwnedRepos
        {
            get
            {
                return _ownedRepos;
            }
        }

        public IEnumerable<Repository> WatchedRepos
        {
            get
            {
                return _watchedRepos;
            }
        }

        public ObservableCollection<Repository> Repos
        {
            get
            {
                return _repos;
            }
            set
            {
                if (_repos != value)
                {
                    _repos = value;
                    _repos.CollectionChanged += (sender, args) =>
                    {
                        var repositories = sender as IEnumerable<Repository>;
                        if (repositories != null && repositories.Any())
                        {
                            foreach (var repository in repositories)
                                repository.CurrentUser = _githubService.Username;
                            _ownedRepos = new ObservableCollection<Repository>(repositories.Where(r => r.IsFrom(_githubService.Username)));
                            _watchedRepos = new ObservableCollection<Repository>(repositories.Except(_ownedRepos));
                        }
                    };
                    RaisePropertyChanged("Repos");
                }
            }
        }

        public FollowingsRequest FollowingsRequest
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

        public FollowersRequest FollowersRequest
        {
            get { return _followersRequest; }
            set
            {
                if (_followersRequest != value)
                {
                    _followersRequest = value;
                    RaisePropertyChanged("FollowersRequest");
                }
            }
        }

        public ObservableCollection<FeaturedRepo> FeaturedRepos
        {
            get { return _featuredRepos; }
            set
            {
                if (_featuredRepos != value)
                {
                    _featuredRepos = value;
                    RaisePropertyChanged("FeaturedRepos");
                }
            }
        }

        public RelayCommand<Feed> FeedSelectedCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<FeaturedRepo> FeaturedRepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PanoramaChangedCommand { get; private set; }

        private void _loadPanel(string header)
        {
            switch (header)
            {
            case "News Feed":
                if (FeedsRequest == null)
                    FeedsRequest = new PrivateFeedsRequest(_githubService.Username);
                break;
            case "Repos":
                if (Repos == null)
                {
                    Repos = _githubService.Load(new WatchedRepoRequest(_githubService.Username));
                    Repos.CollectionChanged += (sender, args) =>
                    {
                        RaisePropertyChanged("WatchedRepos");
                        RaisePropertyChanged("OwnedRepos");
                    };
                }
                break;
            case "Follower":
                if (FollowersRequest == null)
                    FollowersRequest = new FollowersRequest(_githubService.Username);
                break;
            case "Following":
                if (FollowingsRequest == null)
                    FollowingsRequest = new FollowingsRequest(_githubService.Username);
                break;
            case "Profile":
                if (User == null)
                    User = _githubService.Load(new UserRequest(_githubService.Username), u => User = u);
                break;
            case "Explore":
                if (FeaturedRepos == null)
                    FeaturedRepos = _githubService.Load(new FeaturedRepoRequest());
                break;
            default:
                break;
            }
        }

        private void _login()
        {
            IsLoggedIn = true;

            _loadPanel("News Feed");
        }

        private void _logout()
        {
            IsLoggedIn = false;
            User = null;
            FeedsRequest = null;
            Repos = null;
            FollowersRequest = null;
            FollowingsRequest = null;
        }
    }
}