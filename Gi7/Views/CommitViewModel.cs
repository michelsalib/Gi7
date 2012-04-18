using System;
using System.Linq;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using CommitRequest = Gi7.Client.Request.Commit;
using Gi7.Service;
using Gi7.Service.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Gi7.Utils.ViewModels;

namespace Gi7.Views
{
    public class CommitViewModel : ViewModelBase
    {
        private ObservableCollection<CommitFile> _files;
        private bool _minimizeAppBar;
        private bool _canComment;
        private CommitRequest.ListComments _commentsRequest;
        private Push _commit;
        private GithubService _githubService;
        private String _repoName;
        private String _comment;

        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand CommentCommand { get; private set; }
        public RelayCommand RepoSelectedCommand { get; private set; }
        public RelayCommand<SelectionChangedEventArgs> PivotChangedCommand { get; private set; }

        public CommitViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha)
        {
            CanComment = false;
            MinimizeAppBar = true;
            GithubService = githubService;
            RepoName = String.Format("{0}/{1}", username, repo);

            Commit = githubService.Load(new CommitRequest.Get(username, repo, sha), p =>
            {
                Commit = p;

                var files = new ObservableCollection<CommitFile>();
                foreach (var file in p.Files)
                {
                    var lines = new ObservableCollection<CommitLine>();
                    foreach (var line in file.Patch.Split('\n'))
                    {
                        Color color = Colors.White;
                        switch (line.FirstOrDefault())
                        {
                            case '+':
                                color = Colors.Green;
                                break;
                            case '-':
                                color = Colors.Red;
                                break;
                        }

                        lines.Add(new CommitLine
                        {
                            Line = line,
                            Color = new SolidColorBrush(color),
                        });
                    }

                    files.Add(new CommitFile
                    {
                        Lines = lines,
                        File = file,
                    });
                }

                Files = files;
            });

            ShareCommand = new RelayCommand(() =>
            {
                new ShareLinkTask()
                {
                    LinkUri = new Uri("https://github.com" + RepoName + "/commit/" + sha),
                    Title = "Commit on" + RepoName + " is on Github.",
                    Message = "I found this commit on Github, you might want to see it.",
                }.Show();
            });

            CommentCommand = new RelayCommand(() =>
            {
                githubService.Load(new CommitRequest.Comment(username, repo, sha, Comment), r =>
                {
                    Comment = null;
                    CommentsRequest = new CommitRequest.ListComments(username, repo, sha);
                });
            }, () => githubService.IsAuthenticated && _canComment && Comment != null && Comment.Trim().Length > 0);

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
                            CommentsRequest = new CommitRequest.ListComments(username, repo, sha);
                        break;
                    case "Commit":
                        CanComment = false;
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

        public String CommitText
        {
            get
            {
                if (Commit != null && Commit.Stats != null)
                    return String.Format("Showing {0} changed files with {1} additions and {2} deletions.", Commit.Files.Count, Commit.Stats.Additions, Commit.Stats.Deletions);
                else
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

        public CommitRequest.ListComments CommentsRequest
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

        public ObservableCollection<CommitFile> Files
        {
            get { return _files; }
            set
            {
                if (value != _files)
                {
                    _files = value;
                    RaisePropertyChanged("Files");
                }
            }
        }

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
    }
}