using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Request;
using Gi7.Service.Navigation;

namespace Gi7.ViewModel
{
    public class CreateIssueViewModel : ViewModelBase
    {
        private String body;
        private String repoName;
        private String title;

        public CreateIssueViewModel(GithubService githubService, INavigationService navigationService, string user, string repo)
        {
            RepoName = String.Format("{0}/{1}", user, repo);

            CreateIssueCommand = new RelayCommand(() => { githubService.Load(new CreateIssueRequest(user, repo, Title, Body), issue => { navigationService.GoBack(); }); }, () => !String.IsNullOrWhiteSpace(Title) && !String.IsNullOrWhiteSpace(Body));
        }

        public String Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    RaisePropertyChanged("Title");
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
                    RaisePropertyChanged("Body");
                    CreateIssueCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String RepoName
        {
            get { return repoName; }
            set
            {
                if (repoName != value)
                {
                    repoName = value;
                    RaisePropertyChanged("RepoName");
                }
            }
        }

        public RelayCommand CreateIssueCommand { get; private set; }
    }
}