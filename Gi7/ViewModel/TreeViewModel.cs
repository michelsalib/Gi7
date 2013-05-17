using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Service.Navigation;

namespace Gi7.ViewModel
{
    public class TreeViewModel : ViewModelBase
    {
        private String _path;
        private String _repoName;
        private GitTree _tree;

        public RelayCommand<Gi7.Client.Model.GitHubFile> ObjectSelectedCommand { get; private set; }

        public TreeViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha, string path)
        {
            RepoName = String.Format("{0}/{1}", username, repo);
            Path = path;

            Tree = githubService.Load(new Client.Request.Tree.Get(username, repo, sha), t => Tree = t);

            ObjectSelectedCommand = new RelayCommand<Client.Model.GitHubFile>(o =>
            {
                if (o.Type == "blob")
                {
                    navigationService.NavigateTo(String.Format(Service.ViewModelLocator.BlobUrl, username, repo, o.Sha, o.Path));
                }
                else
                { //tree
                    navigationService.NavigateTo(String.Format(Service.ViewModelLocator.TreeUrl, username, repo, o.Sha, o.Path));
                }
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
