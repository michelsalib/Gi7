using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Model;
using Gi7.Service;
using Gi7.Service.Navigation;
using Gi7.Service.Request;

namespace Gi7.Views
{
    public class AboutViewModel : ViewModelBase
    {
        private Repository _Gi7;
        private User _michelsalib;
		private User _albertomonteiro;

        public AboutViewModel(GithubService githubService, INavigationService navigationService)
        {
            Michelsalib = githubService.Load(new UserRequest("michelsalib"), u => Michelsalib = u);
			AlbertoMonteiro = githubService.Load(new UserRequest("albertomonteiro"), u => AlbertoMonteiro = u);
            Gi7 = githubService.Load(new RepositoryRequest("michelsalib", "Gi7"), r => Gi7 = r);

            RepoSelectedCommand = new RelayCommand<Repository>(r =>
            {
                if (r != null)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, r.Owner.Login, r.Name));
            });
            UserSelectedCommand = new RelayCommand<User>(user =>
            {
                if (user != null)
                    navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, user.Login));
            });
        }

        public User Michelsalib
        {
            get { return _michelsalib; }
            set
            {
                if (_michelsalib != value)
                {
                    _michelsalib = value;
                    RaisePropertyChanged("Michelsalib");
                }
            }
        }
		
		public User AlbertoMonteiro
        {
            get { return _albertomonteiro; }
            set
            {
                if (_albertomonteiro != value)
                {
                    _albertomonteiro = value;
                    RaisePropertyChanged("AlbertoMonteiro");
                }
            }
        }

        public Repository Gi7
        {
            get { return _Gi7; }
            set
            {
                if (_Gi7 != value)
                {
                    _Gi7 = value;
                    RaisePropertyChanged("Gi7");
                }
            }
        }

        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
    }
}