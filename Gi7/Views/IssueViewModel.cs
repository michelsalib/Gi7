using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Gi7.Model;

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

        private ObservableCollection<Comment> _comments;
        public ObservableCollection<Comment> Comments
        {
            get { return _comments; }
            set
            {
                if (_comments != value)
                {
                    _comments = value;
                    RaisePropertyChanged("Comments");
                }
            }
        }

        public IssueViewModel(Service.GithubService githubService, string username, string repo, string number)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            IssueName = "Issue #" + number;

            Issue = githubService.GetIssue(username, repo, number, i => Issue = i);
            Comments = githubService.GetIssueComments(username, repo, number);
        }
    }
}
