using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Model;
using Gi7.Service;
using Gi7.Service.Navigation;
using Gi7.Service.Request;
using Microsoft.Phone.Controls;
using Gi7.Service.Request.Base;
using Gi7.Model.Feed.Base;

namespace Gi7.Views
{
    public class UserViewModel : ViewModelBase
    {
        private String _username;
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
                    return Repos.Where(r => r.Owner.Login == Username);
                else
                    return null;
            }
        }

        public IEnumerable<Repository> WatchedRepos
        {
            get
            {
                if (Repos != null)
                    return Repos.Where(r => r.Owner.Login != Username);
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

        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }

        public UserViewModel(GithubService githubService, INavigationService navigationService, string user)
        {
            Username = user;
            FeedsRequest = new FeedsRequest(Username);

            RepoSelectedCommand = new RelayCommand<Repository>(r =>
            {
                if (r != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, r.Owner.Login, r.Name));
            });
            UserSelectedCommand = new RelayCommand<User>(u =>
            {
                if (user != null)
                    navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, u.Login));
            });
            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                    case "Feed":
                        if (FeedsRequest == null)
                        {
                            FeedsRequest = new FeedsRequest(Username);
                        }
                        break;
                    case "Repos":
                        if (Repos == null)
                        {
                            Repos = githubService.Load(new WatchedRepoRequest(Username));
                            Repos.CollectionChanged += (sender, e) =>
                            {
                                RaisePropertyChanged("WatchedRepos");
                                RaisePropertyChanged("OwnedRepos");
                            };
                        }
                        break;
                    case "Users":
                        if (Following == null)
                            Following = githubService.Load(new FollowingsRequest(Username));
                        if (Followers == null)
                            Followers = githubService.Load(new FollowersRequest(Username));
                        break;
                    case "Profile":
                    case "Details":
                        if (User == null)
                            User = githubService.Load(new UserRequest(Username), u => User = u);
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
