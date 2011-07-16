using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Github7.Service;
using Github7.Utils.Messages;
using Github7.Controls;

namespace Github7.Views
{
    public class HomeViewModel : ViewModelBase
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

        public HomeViewModel(GithubService githubService)
        {
            // default loading
            if (githubService.IsAuthenticated)
            {
                _loadConnected();
            }
            else
            {
                _loadDisconnected();
            }

            // when authentication changes
            githubService.IsAuthenticatedChanged += (s, e) =>
            {
                if (e.IsAuthenticated)
                {
                    _loadConnected();
                }
                else
                {
                    _loadDisconnected();
                }
            };

            // listening to logout
            Messenger.Default.Register<bool>(this, "logout", b => githubService.Logout());

            // listening to loading
            githubService.Loading += (s, e) => IsLoading = e.IsLoading;
            IsLoading = githubService.IsLoading;
        }

        private void _loadConnected()
        {
            Messenger.Default.Send<bool>(true, "clearHome");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(FeedsPanel), "News feed"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(RepositoryPanel), "Repos"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(UsersPanel), "Users"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(UserPanel), "Profile"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(AboutPanel), "About"), "homeAdd");
        }

        private void _loadDisconnected()
        {
            Messenger.Default.Send<bool>(true, "clearHome");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(LoginPanel), "Login"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(AboutPanel), "About"), "homeAdd");
        }
    }
}
