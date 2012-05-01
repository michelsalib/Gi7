using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Service.Navigation;
using Gi7.Utils.ContentDetector;
using Blob = Gi7.Client.Request.Tree.Blob;
using TreeRequest = Gi7.Client.Request.Tree;

namespace Gi7.Views
{
    public class BlobViewModel : ViewModelBase
    {
        private String _path;
        private String _repoName;
        private IEnumerable<String> _textFile;

        public BlobViewModel(GithubService githubService, INavigationService navigationService, string username, string repo, string sha, string path)
        {
            Path = path;
            RepoName = String.Format("{0}/{1}", username, repo);

            githubService.Load(new Blob(username, repo, sha), b =>
            {
                byte[] encodedDataAsBytes = Convert.FromBase64String(b.Content);
                String content = Encoding.UTF8.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
                TextFile = content.Split('\n');

                HeaderSignature type = new ContentGuesser().GuessType(path, encodedDataAsBytes);
                if (type != null)
                    Console.Out.WriteLine(type.SignatureName);
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