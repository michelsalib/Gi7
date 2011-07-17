using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model;
using Github7.Service;
using Microsoft.Phone.Controls;
using Github7.Service.Navigation;

namespace Github7.Views
{
    public class UserViewModel : ViewModelBase
    {
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

            User = githubService.GetUser(Username, u => User = u);

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
                    case "Repos":
                        if (Repos == null)
                        {
                            Repos = githubService.GetWatchedRepos(Username);
                            Repos.CollectionChanged += (sender, e) =>
                            {
                                RaisePropertyChanged("WatchedRepos");
                                RaisePropertyChanged("OwnedRepos");
                            };
                        }
                        break;
                    case "Users":
                        if (Following == null)
                            Following = githubService.GetFollowing(Username);
                        if (Followers == null)
                            Followers = githubService.GetFollowers(Username);
                        break;
                    case "Profile":
                        if (User == null)
                            User = githubService.GetUser(Username, u => User = u);
                        break;
                    default:
                        break;
                }
            });

            // listening to loading
            githubService.Loading += (s, e) => IsLoading = e.IsLoading;
            IsLoading = githubService.IsLoading;
        }
    }
}
