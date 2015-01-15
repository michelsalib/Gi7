using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Request.Issue;
using Gi7.Service.Navigation;

namespace Gi7.ViewModel
{
    public class CreateIssueViewModel : ViewModelBase
    {
        private string title;
        private string body;

        private readonly string _user;
        private string _repo;

        private readonly GithubService _githubService;
        private readonly INavigationService _navigationService;

        public CreateIssueViewModel(GithubService githubService, INavigationService navigationService, string user, string repo)
        {
            _githubService = githubService;
            _navigationService = navigationService;

            _user = user;
            _repo = repo;
            CreateIssueCommand = new RelayCommand(CreateIssue);
        }

        private void CreateIssue()
        {
            IssueFunctions.CreateIssue(_githubService.GitHubClient, title, _repo, _user, body);
            _navigationService.GoBack();
        }

        public String Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged();
                    CreateIssueCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String Body
        {
            get { return body; }
            set
            {
                if (body != value)
                {
                    body = value;
                    RaisePropertyChanged();
                    CreateIssueCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String RepoName
        {
            get { return _repo; }
            set
            {
                if (_repo != value)
                {
                    _repo = value;
                    RaisePropertyChanged();
                }
            }
        }

        public RelayCommand CreateIssueCommand { get; private set; }
    }
}