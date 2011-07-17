using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Github7.Service;
using Github7.Utils.Messages;
using Github7.Controls;
using Github7.Model;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using Github7.Model.Feed;
using Github7.Service.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace Github7.Views
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

        private User _michelsalib;
        public User Michelsalib
        {
            get { return _michelsalib; }
            set
            {
                if (_michelsalib != value)
                {
                    _michelsalib = value;
                    RaisePropertyChanged("Michelsalib");
                }
            }
        }

        private Repository _github7;
        public Repository Github7
        {
            get { return _github7; }
            set
            {
                if (_github7 != value)
                {
                    _github7 = value;
                    RaisePropertyChanged("Github7");
                }
            }
        }

        private ObservableCollection<Feed> _feeds;
        public ObservableCollection<Feed> Feeds
        {
            get { return _feeds; }
            set
            {
                if (_feeds != value)
                {
                    _feeds = value;
                    RaisePropertyChanged("Feeds");
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
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, feed.Repository.Owner.Login, feed.Repository.Name));
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

            // listening to logout
            Messenger.Default.Register<bool>(this, "logout", b => githubService.Logout());

            // listening to loading
            githubService.Loading += (s, e) => IsLoading = e.IsLoading;
            IsLoading = githubService.IsLoading;
        }

        private void _loadPanel(string header)
        {
            switch (header)
            {
                case "News Feed":
                    if(Feeds == null)
                        Feeds = _githubService.GetNewsFeed();
                    break;
                case "Repos":
                    if (Repos == null)
                    {
                        Repos = _githubService.GetWatchedRepos(_githubService.Username);
                        Repos.CollectionChanged += (sender, args) =>
                        {
                            RaisePropertyChanged("WatchedRepos");
                            RaisePropertyChanged("OwnedRepos");
                        };
                    }
                    break;
                case "Users":
                    if(Following == null)
                        Following = _githubService.GetFollowing(_githubService.Username);
                    if (Followers == null)
                        Followers = _githubService.GetFollowers(_githubService.Username);
                    break;
                case "Profile":
                    if(User == null)
                        User = _githubService.GetUser(_githubService.Username, u => User = u);
                    break;
                case "About":
                    if(Michelsalib == null)
                        Michelsalib = _githubService.GetUser("michelsalib", u => Michelsalib = u);
                    if(Github7 == null)
                        Github7 = _githubService.GetRepository("michelsalib", "Github7", r => Github7 = r);
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
            Feeds = null;
            Repos = null;
            Followers = null;
            Following = null;
        }
    }
}
