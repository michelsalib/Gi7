using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Model.Event;
using Gi7.Client.Model.Extra;
using Gi7.Client.Request;
using Gi7.Client.Request.Event;
using Gi7.Client.Request.Repository;
using Gi7.Service;
using Gi7.Service.Navigation;
using Gi7.Utils;
using Microsoft.Phone.Controls;
using UserRequest = Gi7.Client.Request.User;

namespace Gi7.Views
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly GithubService _githubService;
        private ObservableCollection<FeaturedRepo> _featuredRepos;
        private ListReceived _eventsRequest;
        private UserRequest.ListFollowers _followersRequest;
        private UserRequest.ListFollowings _followingsRequest;
        private bool _isLoggedIn;
        private ObservableCollection<Repository> _ownedRepos;
        private ObservableCollection<Repository> _repos;
        private User _user;
        private ObservableCollection<Repository> _watchedRepos;

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
            EventSelectedCommand = new RelayCommand<Event>(e =>
            {
                if (e != null)
                    navigationService.NavigateTo(new EventManager().GetDestination(e));
            });
            UserSelectedCommand = new RelayCommand<User>(user =>
            {
                if (user != null)
                    navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, user.Login));
            });
            PanoramaChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args => { _loadPanel((args.AddedItems[0] as PanoramaItem).Header as String); });

            // init
            if (_githubService.IsAuthenticated)
                _login();
            else
                _logout();

            // listenning to the github service
            githubService.IsAuthenticatedChanged += (s, e) =>
            {
                if (e.IsAuthenticated)
                    _login();
                else
                    _logout();
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

        public ListReceived EventsRequest
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
            get { return _ownedRepos; }
        }

        public IEnumerable<Repository> WatchedRepos
        {
            get { return _watchedRepos; }
        }

        public ObservableCollection<Repository> Repos
        {
            get { return _repos; }
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
                            foreach (Repository repository in repositories)
                                repository.CurrentUser = _githubService.Username;
                            _ownedRepos = new ObservableCollection<Repository>(repositories.Where(r => r.IsFrom(_githubService.Username)));
                            _watchedRepos = new ObservableCollection<Repository>(repositories.Except(_ownedRepos));
                        }
                    };
                    RaisePropertyChanged("Repos");
                }
            }
        }

        public UserRequest.ListFollowings FollowingsRequest
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

        public UserRequest.ListFollowers FollowersRequest
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

        public RelayCommand<Event> EventSelectedCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<FeaturedRepo> FeaturedRepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PanoramaChangedCommand { get; private set; }

        private void _loadPanel(string header)
        {
            switch (header)
            {
                case "News Feed":
                    if (EventsRequest == null)
                    {
                        EventsRequest = new ListReceived(_githubService.Username);
                    }
                    break;
                case "Repos":
                    if (Repos == null)
                    {
                        Repos = _githubService.Load(new ListWatched(_githubService.Username));
                        Repos.CollectionChanged += (sender, args) =>
                        {
                            RaisePropertyChanged("WatchedRepos");
                            RaisePropertyChanged("OwnedRepos");
                        };
                    }
                    break;
                case "Follower":
                    if (FollowersRequest == null)
                        FollowersRequest = new UserRequest.ListFollowers(_githubService.Username);
                    break;
                case "Following":
                    if (FollowingsRequest == null)
                        FollowingsRequest = new UserRequest.ListFollowings(_githubService.Username);
                    break;
                case "Profile":
                    if (User == null)
                        User = _githubService.Load(new UserRequest.Get(_githubService.Username), u => User = u);
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
            EventsRequest = null;
            Repos = null;
            FollowersRequest = null;
            FollowingsRequest = null;
        }
    }
}