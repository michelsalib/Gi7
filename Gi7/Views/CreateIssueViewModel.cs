using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request.Repository;
using Gi7.Service;
using Gi7.Service.Navigation;
using UserRequest = Gi7.Client.Request.User;
using Microsoft.Phone.Tasks;
using Gi7.Client.Request.Issue;

namespace Gi7.Views
{
    public class CreateIssueViewModel : ViewModelBase
    {
        private String title;
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

        private String body;
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

        private String repoName;
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

        public CreateIssueViewModel(GithubService githubService, INavigationService navigationService, string user, string repo)
        {
            RepoName = String.Format("{0}/{1}", user, repo);

            CreateIssueCommand = new RelayCommand(() => {
                githubService.Load(new Create(user, repo, Title, Body), issue =>
                {
                    navigationService.GoBack();
                });
            }, () => !String.IsNullOrWhiteSpace(Title) && !String.IsNullOrWhiteSpace(Body));
        }
    }
}