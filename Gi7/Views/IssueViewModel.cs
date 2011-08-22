using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Model;
using Gi7.Service.Request;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using Gi7.Service;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public class IssueViewModel : ViewModelBase
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

        private String _issueName;
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

        private Issue _issue;
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

        private GithubService _githubService;
        public GithubService GithubService
        {
            get { return _githubService; }
            set
            {
                if (_githubService != value)
                {
                    _githubService = value;
                    RaisePropertyChanged("GithubService");
                }
            }
        }

        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }

        public IssueViewModel(Service.GithubService githubService, string username, string repo, string number)
        {
            GithubService = githubService;

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
    }
}
