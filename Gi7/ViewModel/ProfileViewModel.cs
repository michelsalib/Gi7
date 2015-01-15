using GalaSoft.MvvmLight;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Client.Request.Organization;
using System.Collections.ObjectModel;

namespace Gi7.ViewModel
{
    class ProfileViewModel : ViewModelBase
    {
        private readonly GithubService _githubService;
        private User _user;

        public ProfileViewModel(GithubService githubService)
        {
            _githubService = githubService;
            Organizations = new ObservableCollection<Organization>();

            LoadView(githubService);
        }

        public void LoadView(GithubService githubService)
        {
            if (User == null)
            {
                User = _githubService.Load(new UserRequest(_githubService.Username), u =>
                {
                    User = u;
                    _githubService.Load(new UserOrganizationRequest(_githubService.Username), organizations =>
                    {
                        foreach (var organization in organizations)
                        {
                            Organizations.Add(organization);
                        }
                    });
                });
            }
        }


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

        public ObservableCollection<Organization> Organizations { get; set; }
    }
}
