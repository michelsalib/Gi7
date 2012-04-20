using System;
using GalaSoft.MvvmLight;
using Gi7.Client;
using Gi7.Service.Navigation;
using TreeRequest = Gi7.Client.Request.Tree;
using Gi7.Client.Model;
using GalaSoft.MvvmLight.Command;
using Gi7.Service;
using System.Collections.Generic;
using Gi7.Utils.ContentDetector;
using System.IO;
using System.Linq;

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
                String content = System.Text.UTF8Encoding.UTF8.GetString(encodedDataAsBytes, 0, encodedDataAsBytes.Length);
                TextFile = content.Split('\n');

                var type = new ContentGuesser().GuessType(path, encodedDataAsBytes);
                if (type != null)
                {
                    Console.Out.WriteLine(type.SignatureName);
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
