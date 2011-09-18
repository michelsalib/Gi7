using GalaSoft.MvvmLight;
using Gi7.Model;
using Gi7.Service;
using Gi7.Service.Request;

namespace Gi7.Views
{
    public class AboutViewModel : ViewModelBase
    {
        private User _michelsalib;
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

        private Repository _Gi7;
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

        public AboutViewModel(GithubService githubService)
        {
            Michelsalib = githubService.Load(new UserRequest("michelsalib"), u => Michelsalib = u);
            Gi7 = githubService.Load(new RepositoryRequest("michelsalib", "Gi7"), r => Gi7 = r);
        }
    }
}
