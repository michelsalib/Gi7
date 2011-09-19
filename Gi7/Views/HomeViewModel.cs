using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Gi7.Model;
using Gi7.Model.Feed.Base;
using Gi7.Service;
using Gi7.Service.Navigation;
using Gi7.Service.Request;
using Gi7.Service.Request.Base;
using Microsoft.Phone.Controls;
using System.Windows;

namespace Gi7.Views
{
    public class HomeViewModel : ViewModelBase
    {
        private bool _isLoggedIn;
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
            get
            {
                return !IsLoggedIn;
            }
        }

        private User _user;
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

        private IGithubPaginatedRequest<Feed> _feedsRequest;
        public IGithubPaginatedRequest<Feed> FeedsRequest
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
                if (Repos != null)
                    return Repos.Where(r => r.Owner.Login == _githubService.Username);
                else
                    return null;
            }
        }

        public IEnumerable<Repository> WatchedRepos
        {
            get {
                if(Repos != null)
                    return Repos.Where(r => r.Owner.Login != _githubService.Username);
                else
                    return null;
            }
        }

        private ObservableCollection<Repository> _repos;
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

        private ObservableCollection<User> _following;
        public ObservableCollection<User> Following
        {
            get { return _following; }
            set
            {
                if (_following != value)
                {
                    _following = value;
                    RaisePropertyChanged("Following");
                }
            }
        }

        private ObservableCollection<User> _followers;
        public ObservableCollection<User> Followers
        {
            get { return _followers; }
            set
            {
                if (_followers != value)
                {
                    _followers = value;
                    RaisePropertyChanged("Followers");
                }
            }
        }

        public RelayCommand<Feed> FeedSelectedCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PanoramaChangedCommand { get; private set; }
        private readonly GithubService _githubService;

        public HomeViewModel(GithubService githubService, INavigationService navigationService)
        {
            _githubService = githubService;

            // commands
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
            UserSelectedCommand = new RelayCommand<User>(user => {
                if (user != null)
                    navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, user.Login));
            });
            PanoramaChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                _loadPanel((args.AddedItems[0] as PanoramaItem).Header as String);
            });
            
            // init
            if (_githubService.IsAuthenticated)
            {
                _login();
            }
            else
            {
                _logout();
            }

            // listenning to the github service
            githubService.IsAuthenticatedChanged += (s, e) =>
            {
                if (e.IsAuthenticated)
                {
                    _login();
                }
                else
                {
                    _logout();
                }
            };

            // listening to view events
            Messenger.Default.Register<bool>(this, "logout", b => githubService.Logout());
            Messenger.Default.Register<bool>(this, "about", b => navigationService.NavigateTo(ViewModelLocator.AboutUrl));
        }

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
                case "Users":
                    if(Following == null)
                        Following = _githubService.Load(new FollowingsRequest(_githubService.Username));
                    if (Followers == null)
                        Followers = _githubService.Load(new FollowersRequest(_githubService.Username));
                    break;
                case "Profile":
                    if(User == null)
                        User = _githubService.Load(new UserRequest(_githubService.Username), u => User = u);
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
            Followers = null;
            Following = null;
        }
    }
}
