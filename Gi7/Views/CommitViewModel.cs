using System;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Request;
using Microsoft.Phone.Controls;
using Gi7.Client.Model;
using Gi7.Service.Navigation;
using Gi7.Service;

namespace Gi7.Views
{
    public class CommitViewModel : ViewModelBase
    {
        private CommitCommentsRequest _commentsRequest;
        private Push _commit;
        private GithubService _githubService;
        private String _repoName;

        public CommitViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha)
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
        public RelayCommand RepoSelectedCommand { get; private set; }
    }
}