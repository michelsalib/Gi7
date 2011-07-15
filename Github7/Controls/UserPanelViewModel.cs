using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model;
using Github7.Service;

namespace Github7.Controls
{
    public class UserPanelViewModel : ViewModelBase
    {
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

        public UserPanelViewModel(GithubService githubService, string username)
        {
            User = githubService.GetUser(username, u => User = u);
        }
    }
}
