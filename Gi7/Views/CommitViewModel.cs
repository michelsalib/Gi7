using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gi7.Model;
using GalaSoft.MvvmLight;

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

        public CommitViewModel(Service.GithubService GithubService, string username, string repo, string sha)
        {
            RepoName = String.Format("{0}/{1}", username, repo);

            Commit = GithubService.GetCommit(username, repo, sha, p => Commit = p);
        }
    }
}
