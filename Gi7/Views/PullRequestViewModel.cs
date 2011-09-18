using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Model;
using Gi7.Service.Request;
using Microsoft.Phone.Controls;
using Gi7.Service;

namespace Gi7.Views
{
    public class PullRequestViewModel : ViewModelBase
    {
        private String _repoName;
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

        private PullRequest _pullRequest;
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

        private IssueCommentsRequest _commentsRequest;
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

        public PullRequestViewModel(Service.GithubService githubService, string username, string repo, string number)
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
    }
}
