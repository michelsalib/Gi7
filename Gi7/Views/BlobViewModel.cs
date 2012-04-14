using System;
using GalaSoft.MvvmLight;
using Gi7.Client;
using Gi7.Service.Navigation;
using TreeRequest = Gi7.Client.Request.Tree;
using Gi7.Client.Model;
using GalaSoft.MvvmLight.Command;
using Gi7.Service;
using System.Collections.Generic;

namespace Gi7.Views
{
    public class BlobViewModel : ViewModelBase
    {
        private String _path;
        private String _repoName;
        private IEnumerable<String> _textFile;

        public RelayCommand<Gi7.Client.Model.Object> ObjectSelectedCommand { get; private set; }

        public BlobViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha, string path)
        {
            Path = path;
            RepoName = String.Format("{0}/{1}", username, repo);

            githubService.Load(new TreeRequest.Blob(username, repo, sha), b => {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(b.Content);
                TextFile = System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length).Split('\n');
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

        public IEnumerable<String> TextFile
        {
            get { return _textFile; }
            set
            {
                if (_textFile != value)
                {
                    _textFile = value;
                    RaisePropertyChanged("TextFile");
                }
            }
        }
    }
}
