using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Github7.Service;
using Github7.Utils.Messages;
using Github7.Controls;

namespace Github7.Views
{
    public class HomeViewModel : ViewModelBase
    {
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
        }

        private void _loadConnected()
        {
            Messenger.Default.Send<bool>(true, "clearHome");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(FeedsPanel), "News feed"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(RepositoryPanel), "Repos"), "homeAdd");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(UsersPanel), "Users"), "homeAdd"); 
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(UserPanel), "Profile"), "homeAdd");
        }

        private void _loadDisconnected()
        {
            Messenger.Default.Send<bool>(true, "clearHome");
            Messenger.Default.Send<PanelMessage>(new PanelMessage(typeof(LoginPanel), "Login"), "homeAdd");
        }
    }
}
