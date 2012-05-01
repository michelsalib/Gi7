using System;
using System.Collections.ObjectModel;
using Gi7.Client.Model;
using System.Collections.Generic;

namespace Gi7.Resources.DesignData
{
    public class RepositoryDataModel
    {
        public RepositoryDataModel()
        {
            var user = new User
            {
                Login = "michelsalib",
                AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
            };

            Repository = new Repository
            {
                Owner = user,
                Name = "symfony",
                HtmlUrl = "http://github.com/michelsalib/symfony",
                Description = "The Symfony2 PHP framework",
                Watchers = 3,
                Forks = 1,
                Homepage = "symfony.com",
                Parent = new Repository
                {
                    Owner = user,
                    Name = "symfnoy"
                }
            };

            Tree = new GitTree()
            {
                Tree = new List<Gi7.Client.Model.GitHubFile>()
                {
                    new Gi7.Client.Model.GitHubFile() {
                        Path = "readme.md",
                        Type = "blob",
                    },
                    new Gi7.Client.Model.GitHubFile() {
                        Path = "dir",
                        Type = "tree",
                    },
                }
            };

            Commits = new ObservableCollection<Push>
            {
                new Push
                {
                    Author = user,
                    Commit = new Commit
                    {
                        Message = "Removing static access to Username on github service",
                        Author = new Committer
                        {
                            Date = DateTime.Now,
                        }
                    }
                }
            };

            PullRequests = new ObservableCollection<PullRequest>
            {
                new PullRequest
                {
                    Title = "my pull requets",
                    Body = "the description",
                    UpdatedAt = DateTime.Now,
                    State = "Open",
                    User = user,
                }
            };

            Issues = new ObservableCollection<Issue>
            {
                new Issue
                {
                    Title = "my issue",
                    Body = "the description is very long the description is very long the description is very long the description is very long the description is very long the description is very long the description is very long",
                    UpdatedAt = DateTime.Now,
                    State = "Open",
                    User = user,
                }
            };
        }

        public Repository Repository { get; set; }

        public ObservableCollection<Push> Commits { get; set; }

        public ObservableCollection<PullRequest> PullRequests { get; set; }

        public ObservableCollection<Issue> Issues { get; set; }

        public GitTree Tree { get; set; }
    }
}