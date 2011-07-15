using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model;
using Github7.Service;
using Github7.Service.Navigation;

namespace Github7.Controls
{
    public class UsersPanelViewModel : ViewModelBase
    {
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

        public RelayCommand<User> UserSelectedCommand { get; set; }

        public UsersPanelViewModel(GithubService githubService, INavigationService navigationService, string username)
        {
            Followers = githubService.GetFollowers(username);
            Following = githubService.GetFollowing(username);

            UserSelectedCommand = new RelayCommand<User>(u =>{
                if (u != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, u.Login));
            });
        }
    }
}
