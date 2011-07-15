using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model;
using Github7.Service;
using Github7.Service.Navigation;

namespace Github7.Controls
{
    public class RepositoryPanelViewModel : ViewModelBase
    {
        private String _username;

        public IEnumerable<Repository> OwnedRepos
        {
            get { return Repos.Where(r => r.Owner.Login == _username); }
        }

        public IEnumerable<Repository> WatchedRepos
        {
            get { return Repos.Where(r => r.Owner.Login != _username); }
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

        public RelayCommand<Repository> RepoSelectedCommand { get; set; }

        public RepositoryPanelViewModel(GithubService githubService, INavigationService navigationService, string username)
        {
            _username = username;

            Repos = githubService.GetWatchedRepos(_username);
            Repos.CollectionChanged += (s, e) =>
            {
                RaisePropertyChanged("WatchedRepos");
                RaisePropertyChanged("OwnedRepos");
            };

            RepoSelectedCommand = new RelayCommand<Repository>(r =>
            {
                if(r != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, r.Owner.Login, r.Name));
            });
        }
    }
}
