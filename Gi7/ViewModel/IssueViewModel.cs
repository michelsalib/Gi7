using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Service.Navigation;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class IssueViewModel : ViewModelBase
    {
        private Client.Request.Issue.ListComments _commentsRequest;
        private Issue _issue;
        private String _issueName;
        private String _repoName;
        private String _comment;

        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand CommentCommand { get; private set; }

        public IssueViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string number)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            IssueName = "Issue #" + number;

            Issue = githubService.Load(new Client.Request.Issue.Get(username, repo, number), i => Issue = i);
            CommentsRequest = new Client.Request.Issue.ListComments(username, repo, number);

            ShareCommand = new RelayCommand(() =>
            {
                new ShareLinkTask()
                {
                    LinkUri = new Uri(Issue.HtmlUrl),
                    Title = "Issue on" + RepoName + " is on Github: " + Issue.Title,
                    Message = "I found this issue on Github, you might want to see it: " + Issue.Body,
                }.Show();
            }, () => Issue != null);

            RepoSelectedCommand = new RelayCommand(() =>
            {
                navigationService.NavigateTo(String.Format(Service.ViewModelLocator.RepositoryUrl, username, repo));
            });

            CommentCommand = new RelayCommand(() =>
            {
                githubService.Load(new Client.Request.Issue.Comment(username, repo, number, Comment), r =>
                {
                    Comment = null;
                    CommentsRequest = new Client.Request.Issue.ListComments(username, repo, number);
                });
            }, () => githubService.IsAuthenticated && Comment != null && Comment.Trim().Length > 0);
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

        public Client.Request.Issue.ListComments CommentsRequest
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
    }
}