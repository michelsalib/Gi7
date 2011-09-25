using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Model;
using Gi7.Service;
using Gi7.Service.Request;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public class CommitViewModel : ViewModelBase
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

        public String CommitText
        {
            get
            {
                if (Commit.Stats != null)
                    return String.Format("Showing {0} changed files with {1} additions and {2} deletions.", Commit.Files.Count, Commit.Stats.Additions, Commit.Stats.Deletions);
                else
                    return "";
            }
        }

        private Push _commit;
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

        private CommitCommentsRequest _commentsRequest;
        public CommitCommentsRequest CommentsRequest
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

        public CommitViewModel(Service.GithubService githubService, string username, string repo, string sha)
        {
            GithubService = githubService;

            RepoName = String.Format("{0}/{1}", username, repo);

            Commit = githubService.Load(new CommitRequest(username, repo, sha), p => Commit = p);

            PivotChangedCommand = new RelayCommand<SelectionChangedEventArgs>(args =>
            {
                var header = (args.AddedItems[0] as PivotItem).Header as String;
                switch (header)
                {
                    case "Comments":
                        if (CommentsRequest == null)
                            CommentsRequest = new CommitCommentsRequest(username, repo, sha);
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
