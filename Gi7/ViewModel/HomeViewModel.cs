using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Model.Event;
using Gi7.Client.Model.Extra;
using Gi7.Client.Request;
using Gi7.Client.Request.Event;
using Gi7.Client.Request.Repository;
using Gi7.Client.Request.User;
using Gi7.Service.Navigation;
using Gi7.Utils;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Gi7.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly GithubService _githubService;
        private ListReceived _eventsRequest;
        private ListFollowers _followersRequest;
        private ListFollowings _followingsRequest;
        private User _user;
        private String _search;
        private bool _isLoggedIn;

        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand<Event> EventSelectedCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<FeaturedRepo> FeaturedRepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PanoramaChangedCommand { get; private set; }
        public RelayCommand<SearchResult> ResultSelectedCommand { get; private set; }

        public HomeViewModel(GithubService githubService, INavigationService navigationService)
        {
            _githubService = githubService;

            // commands
            FeaturedRepoSelectedCommand = new RelayCommand<FeaturedRepo>(r => OnFeaturedRepoSelected(navigationService, r));
            RepoSelectedCommand = new RelayCommand<Repository>(r => OnRepoSelected(navigationService, r));
            EventSelectedCommand = new RelayCommand<Event>(e => OnEventSelected(navigationService, e));
            UserSelectedCommand = new RelayCommand<User>(user => OnUserSelected(navigationService, user));
            ResultSelectedCommand = new RelayCommand<SearchResult>(r => OnResultSelected(navigationService, r));
            PanoramaChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnPanoramaChanged);
            AboutCommand = new RelayCommand(() => OnAbout(navigationService));
            LogoutCommand = new RelayCommand(() => githubService.Logout(), () => IsLoggedIn);

            // init
            if (_githubService.IsAuthenticated)
                Login();
            else
                Logout();

            // listenning to the github service
            githubService.IsAuthenticatedChanged += (s, e) => OnIsAuthenticatedChanged(e);

            // listenning to the search box
            PropertyChanged += (s, e) => OnPropertyChanged(githubService, e);

            WatchedRepos = new ObservableCollection<Repository>();
            OwnedRepos = new ObservableCollection<Repository>();
        }

        private void OnPropertyChanged(GithubService githubService, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Search")
                SearchResults = githubService.Load(new Search(Search), r => SearchResults = r);
        }

        private void OnIsAuthenticatedChanged(AuthenticatedEventArgs e)
        {
            if (e.IsAuthenticated)
                Login();
            else
                Logout();
        }

        private static void OnAbout(INavigationService navigationService)
        {
            navigationService.NavigateTo(Service.ViewModelLocator.AboutUrl);
        }

        private void OnPanoramaChanged(SelectionChangedEventArgs args)
        {
            LoadPanel((args.AddedItems[0] as PanoramaItem).Header as String);
        }

        private static void OnResultSelected(INavigationService navigationService, SearchResult r)
        {
            if (r.Type == "user")
                navigationService.NavigateTo(string.Format(Service.ViewModelLocator.UserUrl, r.Name));
            else // repo
            {
                var repoData = r.Name.Split('/');
                navigationService.NavigateTo(string.Format(Service.ViewModelLocator.RepositoryUrl, repoData[0].Trim(), repoData[1].Trim()));
            }
        }

        private static void OnUserSelected(INavigationService navigationService, User user)
        {
            if (user != null)
                navigationService.NavigateTo(string.Format(Service.ViewModelLocator.UserUrl, user.Login));
        }

        private static void OnEventSelected(INavigationService navigationService, Event e)
        {
            if (e != null)
                navigationService.NavigateTo(new EventManager().GetDestination(e));
        }

        private static void OnRepoSelected(INavigationService navigationService, Repository r)
        {
            if (r != null)
                navigationService.NavigateTo(String.Format(Service.ViewModelLocator.RepositoryUrl, r.Owner.Login, r.Name));
        }

        private static void OnFeaturedRepoSelected(INavigationService navigationService, FeaturedRepo r)
        {
            if (r != null)
                navigationService.NavigateTo(String.Format(Service.ViewModelLocator.RepositoryUrl, r.User, r.Repo));
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
                    LogoutCommand.RaiseCanExecuteChanged();
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

        public ObservableCollection<Repository> OwnedRepos { get; set; }

        public ObservableCollection<Repository> WatchedRepos { get; set; }

        public ListFollowings FollowingsRequest
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

        public ListFollowers FollowersRequest
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
            get;
            set;
        }

        public ObservableCollection<SearchResult> SearchResults
        {
            get;
            set;
        }

        public String Search
        {
            get { return _search; }
            set
            {
                if (_search != value)
                {
                    _search = value;
                    RaisePropertyChanged("Search");
                }
            }
        }

        private void LoadPanel(string header)
        {
            switch (header)
            {
                case "News Feed":
                    if (EventsRequest == null)
                        EventsRequest = new ListReceived(_githubService.Username);
                    break;
                case "Repos":
                    if (!OwnedRepos.Any())
                        _githubService.Load(new List(), repositories =>
                        {
                            foreach (var repository in repositories)
                                OwnedRepos.Add(repository);
                        });
                    if (!WatchedRepos.Any())
                        _githubService.Load(new ListWatched(_githubService.Username), result =>
                        {
                            foreach (var repository in result.Where(repo => !repo.Owner.Login.Equals(_githubService.Username, StringComparison.InvariantCultureIgnoreCase)))
                                WatchedRepos.Add(repository);
                        });
                    break;
                case "Follower":
                    if (FollowersRequest == null)
                        FollowersRequest = new ListFollowers(_githubService.Username);
                    break;
                case "Following":
                    if (FollowingsRequest == null)
                        FollowingsRequest = new ListFollowings(_githubService.Username);
                    break;
                case "Profile":
                    if (User == null)
                        User = _githubService.Load(new Client.Request.User.Get(_githubService.Username), u => User = u);
                    break;
                case "Explore":
                    if (FeaturedRepos == null)
                        FeaturedRepos = _githubService.Load(new FeaturedRepoRequest());
                    break;
            }
        }

        private void Login()
        {
            IsLoggedIn = true;

            LoadPanel("News Feed");
        }

        private void Logout()
        {
            IsLoggedIn = false;
            User = null;
            EventsRequest = null;
            OwnedRepos = null;
            WatchedRepos = null;
            FollowersRequest = null;
            FollowingsRequest = null;
        }
    }
}