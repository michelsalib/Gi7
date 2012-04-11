using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Service;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Gi7.Client.Request.PullRequest;

namespace Gi7.Views
{
    public class PullRequestViewModel : ViewModelBase
    {
        private ListComments _commentsRequest;
        private PullRequest _pullRequest;
        private String _repoName;

        public PullRequestViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string number)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            PullRequest = githubService.Load(new Get(username, repo, number), pr => PullRequest = pr);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                case "Comments":
                    if (CommentsRequest == null)
                        CommentsRequest = new ListComments(username, repo, number);
                    break;
                default:
                    break;
                }
            });

            RepoSelectedCommand = new RelayCommand(() =>
            {
                navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, username, repo));
            });
        }

        public String RepoName
        {
            get { return _repoName; }
            set
            {
                if (_repoName != value)
                {
                    _repoName = value;
                    RaisePropertyChanged("RepoName");
                }
            }
        }

        public PullRequest PullRequest
        {
            get { return _pullRequest; }
            set
            {
                if (_pullRequest != value)
                {
                    _pullRequest = value;
                    RaisePropertyChanged("PullRequest");
                }
            }
        }

        public ListComments CommentsRequest
        {
            get { return _commentsRequest; }
            set
            {
                if (_commentsRequest != value)
                {
                    _commentsRequest = value;
                    RaisePropertyChanged("CommentsRequest");
                }
            }
        }

        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }
    }
}