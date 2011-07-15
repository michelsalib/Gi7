using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model.Feed;
using Github7.Service;
using Github7.Service.Navigation;

namespace Github7.Controls
{
    public class FeedsPanelViewModel : ViewModelBase
    {
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

        public RelayCommand<Feed> FeedSelectedCommand { get; private set; }

        public FeedsPanelViewModel(GithubService githubService, INavigationService navigationService)
        {
            Feeds = githubService.GetNewsFeed();

            FeedSelectedCommand = new RelayCommand<Feed>(feed =>
            {
                if(feed != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, feed.Repository.Owner.Login, feed.Repository.Name));
            });
        }
    }
}
