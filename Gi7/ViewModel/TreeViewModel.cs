using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request;
using Gi7.Service.Navigation;

namespace Gi7.ViewModel
{
    public class TreeViewModel : ViewModelBase
    {
        private String _path;
        private String _repoName;
        private GitTree _tree;

        public TreeViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha, string path)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            Path = path;

            Tree = githubService.Load(new TreeRequest(username, repo, sha), t => Tree = t);

            ObjectSelectedCommand = new RelayCommand<GitHubFile>(o =>
            {
                if (o.Type == "blob")
                    navigationService.NavigateTo(String.Format(ViewModelLocator.BLOB_URL, username, repo, o.Sha, o.Path));
                else
                    //tree
                    navigationService.NavigateTo(String.Format(ViewModelLocator.TREE_URL, username, repo, o.Sha, o.Path));
            });
        }

        public RelayCommand<GitHubFile> ObjectSelectedCommand { get; private set; }

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

        public String Path
        {
            get { return _path; }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    RaisePropertyChanged("Path");
                }
            }
        }

        public GitTree Tree
        {
            get { return _tree; }
            set
            {
                if (_tree != value)
                {
                    _tree = value;
                    RaisePropertyChanged("Tree");
                }
            }
        }
    }
}