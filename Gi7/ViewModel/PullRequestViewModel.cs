using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class PullRequestViewModel : ViewModelBase
    {
        private bool _canComment;
        private String _comment;
        private bool _minimizeAppBar;
        private PullRequest _pullRequest;
        private String _pullRequestName;
        private String _repoName;
        private PullRequestCommentsRequest commentsRequest;

        public PullRequestViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string number)
        {
            if (!IsInDesignMode)
            {
                CanComment = false;
                MinimizeAppBar = true;
                RepoName = String.Format("{0}/{1}", username, repo);
                PullRequestName = "Pull Request #" + number; 
            }

            PullRequest = githubService.Load(new PullRequestRequest(username, repo, number), pr => PullRequest = pr);

            ShareCommand = new RelayCommand(() => new ShareLinkTask
            {
                LinkUri = new Uri(PullRequest.HtmlUrl),
                Title = "Pull Request on" + RepoName + " is on Github: " + PullRequest.Title,
                Message = "I found this pull request on Github, you might want to see it: " + PullRequest.Body,
            }.Show(), () => PullRequest != null);

            CommentCommand = new RelayCommand(() => githubService.Load(new CreatePullRequestCommentsRequest(username, repo, number, Comment), r =>
            {
                Comment = null;
                CommentsRequest = new PullRequestCommentsRequest(username, repo, number);
            }), () => githubService.IsAuthenticated && _canComment && Comment != null && Comment.Trim().Length > 0);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                MinimizeAppBar = true;
                CanComment = false;
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                case "Comments":
                    MinimizeAppBar = false;
                    CanComment = true;
                    if (CommentsRequest == null)
                        CommentsRequest = new PullRequestCommentsRequest(username, repo, number);
                    break;
                default: // main pivot
                    CanComment = false;
                    break;
                }
            });

            RepoSelectedCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.REPOSITORY_URL, username, repo)));
        }

        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }
        public RelayCommand CommentCommand { get; private set; }
        public RelayCommand ShareCommand { get; private set; }

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
                    ShareCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public PullRequestCommentsRequest CommentsRequest
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

        public String PullRequestName
        {
            get { return _pullRequestName; }
            set
            {
                if (_pullRequestName != value)
                {
                    _pullRequestName = value;
                    RaisePropertyChanged("PullRequestName");
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

        public bool MinimizeAppBar
        {
            get { return _minimizeAppBar; }
            set
            {
                if (value != _minimizeAppBar)
                {
                    _minimizeAppBar = value;
                    RaisePropertyChanged("MinimizeAppBar");
                }
            }
        }

        public bool CanComment
        {
            get { return _canComment; }
            set
            {
                if (value != _canComment)
                {
                    _canComment = value;
                    RaisePropertyChanged("CanComment");
                    CommentCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}