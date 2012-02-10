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
    public class IssueViewModel : ViewModelBase
    {
        private IssueCommentsRequest _commentsRequest;
        private Issue _issue;
        private String _issueName;
        private String _repoName;

        public IssueViewModel(GithubService githubService, string username, string repo, string number)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            IssueName = "Issue #" + number;

            Issue = githubService.Load(new IssueRequest(username, repo, number), i => Issue = i);

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

        public String IssueName
        {
            get { return _issueName; }
            set
            {
                if (_issueName != value)
                {
                    _issueName = value;
                    RaisePropertyChanged("IssueName");
                }
            }
        }

        public Issue Issue
        {
            get { return _issue; }
            set
            {
                if (_issue != value)
                {
                    _issue = value;
                    RaisePropertyChanged("Issue");
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