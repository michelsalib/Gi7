using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Service.Navigation;
using Gi7.Utils.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Gi7.ViewModel
{
    public class CommitViewModel : ViewModelBase
    {
        private bool _canComment;
        private String _comment;
        private Push _commit;
        private GithubService _githubService;
        private bool _minimizeAppBar;
        private String _repoName;
        private CommitCommentsRequest commentsRequestRequest;

        public CommitViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha)
        {
            CanComment = false;
            MinimizeAppBar = true;
            GithubService = githubService;
            RepoName = String.Format("{0}/{1}", username, repo);
            Files = new ObservableCollection<CommitFile>();

            Commit = githubService.Load(new CommitRequest(username, repo, sha), p =>
            {
                Commit = p;

                foreach (var file in p.Files)
                {
                    var lines = new ObservableCollection<CommitLine>();
                    if (file.Patch != null)
                        foreach (var line in file.Patch.Split('\n'))
                        {
                            var color = Colors.White;
                            switch (line.FirstOrDefault())
                            {
                            case '+':
                                color = Color.FromArgb(255, 49, 154, 49);
                                break;
                            case '-':
                                color = Color.FromArgb(255, 230, 20, 0);
                                break;
                            case '@':
                                color = Color.FromArgb(255, 25, 162, 222);
                                break;
                            }

                            lines.Add(new CommitLine {Line = line, Color = new SolidColorBrush(color)});
                        }
                    else
                        lines.Add(new CommitLine {Line = "Binary file not shown", Color = new SolidColorBrush(Colors.Gray)});

                    Files.Add(new CommitFile {Lines = lines, File = file,});
                }
            });

            ShareCommand = new RelayCommand(() => new ShareLinkTask
            {
                LinkUri = new Uri("https://github.com" + RepoName + "/commit/" + sha),
                Title = "Commit on" + RepoName + " is on Github.",
                Message = "I found this commit on Github, you might want to see it.",
            }.Show());

            CommentCommand = new RelayCommand(() => githubService.Load(new CommentCommitRequest(username, repo, sha, Comment), r => OnComment(username, repo, sha)), UserCanComment);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args => OnPivotChangedCommand(username, repo, sha, args));

            RepoSelectedCommand = new RelayCommand(() => navigationService.NavigateTo(String.Format(ViewModelLocator.REPOSITORY_URL, username, repo)));
        }

        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand CommentCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }

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

        public String CommitText
        {
            get
            {
                if (Commit != null && Commit.Stats != null)
                    return String.Format("Showing {0} changed files with {1} additions and {2} deletions.", Commit.Files.Count, Commit.Stats.Additions, Commit.Stats.Deletions);
                return "";
            }
        }

        public Push Commit
        {
            get { return _commit; }
            set
            {
                if (_commit != value)
                {
                    _commit = value;
                    RaisePropertyChanged("Commit");
                    RaisePropertyChanged("CommitText");
                }
            }
        }

        public CommitCommentsRequest CommentsRequestRequest
        {
            get { return commentsRequestRequest; }
            set
            {
                if (commentsRequestRequest != value)
                {
                    commentsRequestRequest = value;
                    RaisePropertyChanged("CommentsRequest");
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

        public ObservableCollection<CommitFile> Files { get; set; }

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

        private void OnComment(string username, string repo, string sha)
        {
            Comment = null;
            CommentsRequestRequest = new CommitCommentsRequest(username, repo, sha);
        }

        private bool UserCanComment()
        {
            return GithubService.IsAuthenticated && _canComment && Comment != null && Comment.Trim().Length > 0;
        }

        private void OnPivotChangedCommand(string username, string repo, string sha, SelectionChangedEventArgs args)
        {
            MinimizeAppBar = true;
            CanComment = false;
            var header = (args.AddedItems[0] as PivotItem).Header as String;
            switch (header)
            {
            case "Comments":
                MinimizeAppBar = false;
                CanComment = true;
                if (CommentsRequestRequest == null)
                    CommentsRequestRequest = new CommitCommentsRequest(username, repo, sha);
                break;
            case "Commit":
                CanComment = false;
                break;
            }
        }
    }
}