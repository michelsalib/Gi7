using System;
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
        private BranchesRequest _branchesRequest;
        private CommitsRequest _commitsRequest;
        private CollaboratorRequest _collaboratorRequest;
        private IssuesRequest _issuesRequest;
        private PullRequestsRequest _pullRequestsRequest;
        private Repository _repository;
        private string branch;

        public RepositoryViewModel(GithubService githubService, INavigationService navigationService, String user, String repo)
        {
            branch = "master";
            Repository = githubService.Load(new RepositoryRequest(user, repo), r => Repository = r);

            OwnerCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, Repository.Owner.Login)));

            if (BranchesRequest == null)
                BranchesRequest = new BranchesRequest(user, repo);

            BranchChangedCommand = new RelayCommand<ListPicker>(e =>
            {
                if (e != null)
                {
                    if (e.SelectedItem != null)
                        branch = ((Branch)e.SelectedItem).Name;
                    CommitsRequest = null;
                }
            });

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = ((PivotItem)args.AddedItems[0]).Header as String;
                switch (header)
                {
                    case "Commits":
                        if (CommitsRequest == null)
                            CommitsRequest = new CommitsRequest(user, repo, branch);
                        break;
                    case "Pull requests":
                        if (PullRequestsRequest == null)
                            PullRequestsRequest = new PullRequestsRequest(user, repo);
                        break;
                    case "Issues":
                        if (IssuesRequest == null)
                            IssuesRequest = new IssuesRequest(user, repo);
                        break;
                    case "Collaborators":
                        if (CollaboratorRequest == null)
                            CollaboratorRequest = new CollaboratorRequest(user, repo);
                        break;
                }
            });
            CommitSelectedCommand = new RelayCommand<Push>(push =>
            {
                if (push)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.CommitUrl, Repository.Owner.Login, Repository.Name, push.Sha));
            });
            PullRequestSelectedCommand = new RelayCommand<PullRequest>(pullRequest =>
            {
                if (pullRequest)
                    navigationService.NavigateTo(String.Format(ViewModelLocator.PullRequestUrl, Repository.Owner.Login, Repository.Name, pullRequest.Number));
            });
            IssueSelectedCommand = new RelayCommand<Issue>(issue =>
            {
                if (issue)
                {
                    var destination = issue.PullRequest.HtmlUrl == null ? ViewModelLocator.IssueUrl : ViewModelLocator.PullRequestUrl;
                    navigationService.NavigateTo(String.Format(destination, Repository.Owner.Login, Repository.Name, issue.Number));
                }
            });
        }

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

        public BranchesRequest BranchesRequest
        {
            get { return _branchesRequest; }
            set
            {
                if (_branchesRequest != value)
                {
                    _branchesRequest = value;
                    RaisePropertyChanged("BranchesRequest");
                }
            }
        }

        public CollaboratorRequest CollaboratorRequest
        {
            get { return _collaboratorRequest; }
            set
            {
                if (_collaboratorRequest != value)
                {
                    _collaboratorRequest = value;
                    RaisePropertyChanged("CollaboratorRequest");
                }
            }
        }

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
        public RelayCommand<ListPicker> BranchChangedCommand { get; private set; }
        public RelayCommand<Push> CommitSelectedCommand { get; private set; }
        public RelayCommand<PullRequest> PullRequestSelectedCommand { get; private set; }
        public RelayCommand<Issue> IssueSelectedCommand { get; private set; }
    }
}