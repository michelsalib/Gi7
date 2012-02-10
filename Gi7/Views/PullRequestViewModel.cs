using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public class PullRequestViewModel : ViewModelBase
    {
        private IssueCommentsRequest _commentsRequest;
        private PullRequest _pullRequest;
        private String _repoName;

        public PullRequestViewModel(GithubService githubService, string username, string repo, string number)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            PullRequest = githubService.Load(new PullRequestRequest(username, repo, number), pr => PullRequest = pr);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                case "Comments":
                    if (CommentsRequest == null)
                        CommentsRequest = new IssueCommentsRequest(username, repo, number);
                    break;
                default:
                    break;
                }
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

        public IssueCommentsRequest CommentsRequest
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
    }
}