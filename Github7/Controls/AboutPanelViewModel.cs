using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Github7.Model;
using Github7.Service;
using Github7.Service.Navigation;

namespace Github7.Controls
{
    public class AboutPanelViewModel : ViewModelBase
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

        private Repository _repository;
        public Repository Repository
        {
            get { return _repository; }
            set
            {
                if (_repository != value)
                {
                    _repository = value;
                    RaisePropertyChanged("Repository");
                }
            }
        }

        public RelayCommand UserSelectedCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }

        public AboutPanelViewModel(GithubService githubService, INavigationService navigationService)
        {
            User = githubService.GetUser("michelsalib", u => User = u);

            Repository = githubService.GetRepository("michelsalib", "Github7", r => Repository = r);

            UserSelectedCommand = new RelayCommand(() => navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, "michelsalib")));
            RepoSelectedCommand = new RelayCommand(() => navigationService.NavigateTo(string.Format(ViewModelLocator.RepositoryUrl, "michelsalib", "Github7")));
        }
    }
}
