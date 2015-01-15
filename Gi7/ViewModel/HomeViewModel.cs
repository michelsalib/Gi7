using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Model.Event;
using Gi7.Client.Model.Extra;
using Gi7.Client.Request;
using Gi7.Client.Request.Organization;
using Gi7.Service.Navigation;
using Gi7.Utils;
using Microsoft.Phone.Controls;
using System.Diagnostics;

namespace Gi7.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly GithubService _githubService;
        private UserReceivedEventsRequest _eventsRequest;
        private UserFollowingRequest _followingsRequest;
        private bool _isLoggedIn;
        private string _search;
        private User _user;
        private SearchResult searchResult;
        private UserFollowersRequest followersRequest;
        private RepositoriesRequest _repositoriesRequest;

        public HomeViewModel(GithubService githubService, INavigationService navigationService)
        {
            _githubService = githubService;

            Organizations = new ObservableCollection<Organization>();

            // commands
            RepoSelectedCommand = new RelayCommand<Repository>(r => OnRepoSelected(navigationService, r));
            SearchedRepoSelectedCommand = new RelayCommand<SearchedRepository>(r => OnSearchedRepoSelected(navigationService, r));
            EventSelectedCommand = new RelayCommand<Event>(e => OnEventSelected(navigationService, e));
            UserSelectedCommand = new RelayCommand<User>(user => OnUserSelected(navigationService, user));
            PanoramaChangedCommand = new RelayCommand<SelectionChangedEventArgs>(OnPanoramaChanged);
            ProfileCommand = new RelayCommand(() => OnProfile(navigationService), () => IsLoggedIn);
            AboutCommand = new RelayCommand(() => OnAbout(navigationService));
            LogoutCommand = new RelayCommand(githubService.Logout, () => IsLoggedIn);

            // init
            if (_githubService.IsAuthenticated)
            {
                Login();
            }

            else
            {
                Logout();
            }

            // listenning to the github service
            githubService.IsAuthenticatedChanged += (s, e) => OnIsAuthenticatedChanged(e);

            // listenning to the search box
            PropertyChanged += (s, e) => OnPropertyChanged(githubService, e);

            Repos = new ObservableCollection<Repository>();
        }

        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand ProfileCommand { get; private set; }
        public RelayCommand<Event> EventSelectedCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<SearchedRepository> SearchedRepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PanoramaChangedCommand { get; private set; }

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

        public UserReceivedEventsRequest EventsRequest
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

        public ObservableCollection<Repository> Repos { get; set; }

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

        public SearchResult SearchResult
        {
            get { return searchResult; }
            set
            {
                if (searchResult != value)
                {
                    searchResult = value;
                    RaisePropertyChanged("SearchResult");
                }
            }
        }

        public string Search
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

        private void OnPropertyChanged(GithubService githubService, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Search")
                SearchResult = githubService.Load(new SearchRequest(Search), r => SearchResult = r);
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
            navigationService.NavigateTo(ViewModelLocator.ABOUT_URL);
        }

        private static void OnProfile(INavigationService navigationService)
        {
            navigationService.NavigateTo(ViewModelLocator.PROFILE_URL);
        }

        private void OnPanoramaChanged(SelectionChangedEventArgs args)
        {
            LoadPanel((args.AddedItems[0] as PanoramaItem).Header as string);
        }

        private static void OnUserSelected(INavigationService navigationService, User user)
        {
            if (user != null)
                navigationService.NavigateTo(string.Format(ViewModelLocator.USER_URL, user.Login));
        }

        private static void OnEventSelected(INavigationService navigationService, Event e)
        {
            if (e != null)
                navigationService.NavigateTo(new EventManager().GetDestination(e));
        }

        private static void OnRepoSelected(INavigationService navigationService, Repository r)
        {
            if (r != null)
                navigationService.NavigateTo(string.Format(ViewModelLocator.REPOSITORY_URL, r.Owner.Login, r.Name));
        }

        private static void OnSearchedRepoSelected(INavigationService navigationService, SearchedRepository r)
        {
            if (r != null)
                navigationService.NavigateTo(string.Format(ViewModelLocator.REPOSITORY_URL, r.Username, r.Name));
        }

        private void LoadPanel(string header)
        {
            switch (header)
            {
                case "news":
                    if (EventsRequest == null)
                        EventsRequest = new UserReceivedEventsRequest(_githubService.Username);
                    break;
                case "repositories":
                    if (RepositoriesRequest == null)
                        RepositoriesRequest = new RepositoriesRequest();
                    break;
                case "followers":
                    if (FollowersRequest == null)
                        FollowersRequest = new UserFollowersRequest(_githubService.Username);
                    break;
                case "following":
                    if (FollowingsRequest == null)
                        FollowingsRequest = new UserFollowingRequest(_githubService.Username);
                    break;
            }
        }

        public ObservableCollection<Organization> Organizations { get; set; }

        private void Login()
        {
            IsLoggedIn = true;

            LoadPanel("news");
        }

        private void Logout()
        {
            IsLoggedIn = false;

            User = null;
            EventsRequest = null;
            Repos = null;
            FollowersRequest = null;
            FollowingsRequest = null;
        }

        public RepositoriesRequest RepositoriesRequest
        {
            get { return _repositoriesRequest; }
            set
            {
                if (_repositoriesRequest != value)
                {
                    _repositoriesRequest = value;
                    RaisePropertyChanged("RepositoriesRequest");
                }
            }
        }
    }
}