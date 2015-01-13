using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Client.Request.Issue;
using Gi7.Service.Navigation;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class IssueViewModel : ViewModelBase
    {
        private String _comment;
        private Issue _issue;
        private String _issueName;
        private String _repoName;
        private IssueCommentsRequest commentsRequest;

        public IssueViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string number)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            IssueName = "Issue #" + number;

            Issue = githubService.Load(new IssueRequest(username, repo, number), i => Issue = i);
            CommentsRequest = new IssueCommentsRequest(username, repo, number);

            ShareCommand = new RelayCommand(() =>
            {
                new ShareLinkTask
                {
                    LinkUri = new Uri(Issue.HtmlUrl),
                    Title = "Issue on" + RepoName + " is on Github: " + Issue.Title,
                    Message = "I found this issue on Github, you might want to see it: " + Issue.Body,
                }.Show();
            }, () => Issue != null);

            RepoSelectedCommand = new RelayCommand(() => { navigationService.NavigateTo(String.Format(ViewModelLocator.REPOSITORY_URL, username, repo)); });

            CommentCommand = new RelayCommand(() =>
            {
                githubService.Load(new IssueCommentRequest(username, repo, number, Comment), r =>
                {
                    Comment = null;
                    CommentsRequest = new IssueCommentsRequest(username, repo, number);
                });
            }, () => githubService.IsAuthenticated && Comment != null && Comment.Trim().Length > 0);
        }

        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand CommentCommand { get; private set; }

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
                    ShareCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    RaisePropertyChanged("Comment");
                    CommentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public IssueCommentsRequest CommentsRequest
        {
            get { return commentsRequest; }
            set
            {
                if (commentsRequest != value)
                {
                    commentsRequest = value;
                    RaisePropertyChanged("CommentsRequest");
                }
            }
        }
    }
}