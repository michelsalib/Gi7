using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Model;
using Gi7.Service;
using Gi7.Service.Navigation;
using Gi7.Service.Request;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public class RepositoryViewModel : ViewModelBase
    {
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

        private CommitsRequest _commitsRequest;
        public CommitsRequest CommitsRequest
        {
            get { return _commitsRequest; }
            set
            {
                if (_commitsRequest != value)
                {
                    _commitsRequest = value;
                    RaisePropertyChanged("CommitsRequest");
                }
            }
        }

        private PullRequestsRequest _pullRequestsRequest;
        public PullRequestsRequest PullRequestsRequest
        {
            get { return _pullRequestsRequest; }
            set
            {
                if (_pullRequestsRequest != value)
                {
                    _pullRequestsRequest = value;
                    RaisePropertyChanged("PullRequestsRequest");
                }
            }
        }

        private IssuesRequest _issuesRequest;
        public IssuesRequest IssuesRequest
        {
            get { return _issuesRequest; }
            set
            {
                if (_issuesRequest != value)
                {
                    _issuesRequest = value;
                    RaisePropertyChanged("IssuesRequest");
                }
            }
        }

        public RelayCommand OwnerCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }
        public RelayCommand<PullRequest> PullRequestSelectedCommand { get; private set; }
        public RelayCommand<Issue> IssueSelectedCommand { get; private set; }

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, String user, String repo)
        {
            Repository = githubService.Load(new RepositoryRequest(user, repo), r => Repository = r);

            OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, Repository.Owner.Login)));
            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                    case "Commits":
                        if(CommitsRequest == null)
                            CommitsRequest = new CommitsRequest(user, repo);
                        break;
                    case "Pull requests":
                        if (PullRequestsRequest == null)
                            PullRequestsRequest = new PullRequestsRequest(user, repo);
                        break;
                    case "Issues":
                        if (IssuesRequest == null)
                            IssuesRequest = new IssuesRequest(user, repo);
                        break;
                    default:
                        break;
                }
            });
            CommitSelectedCommand = new RelayCommand<Push>(push =>
            {
                if (push != null)
                {
                    navigationService.NavigateTo(String.Format(ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, push.Sha));
                }
            });
            PullRequestSelectedCommand = new RelayCommand<PullRequest>(pullRequest =>
            {
                if (pullRequest != null)
                {
                    navigationService.NavigateTo(String.Format(ViewModelLocator.PullRequestUrl, Repository.Owner.Login, Repository.Name, pullRequest.Number));
                }
            });
            IssueSelectedCommand = new RelayCommand<Issue>(issue =>
            {
                if (issue != null)
                {
                    var destination = issue.PullRequest.HtmlUrl == null ? ViewModelLocator.IssueUrl : ViewModelLocator.PullRequestUrl;
                    navigationService.NavigateTo(String.Format(destination, Repository.Owner.Login, Repository.Name, issue.Number));
                }
            });
        }
    }
}
