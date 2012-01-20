using System;
using System.Collections.ObjectModel;
using Gi7.Model;

namespace Gi7.Resources.DesignData
{
    public class RepositoryDataModel
    {
        public RepositoryDataModel()
        {
            Repository = new Repository
            {
                Owner = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
                Name = "symfony",
                HtmlUrl = "http://github.com/michelsalib/symfony",
                Description = "The Symfony2 PHP framework",
                Watchers = 3,
                Forks = 1,
                Homepage = "symfony.com",
                Parent = new Repository
                {
                    Owner = new User
                    {
                        Login = "symfony",
                        AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317"
                    },
                    Name = "symfnoy"
                }
            };

            Commits = new ObservableCollection<Push>();
            Commits.Add(new Push
            {
                Author = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
                Commit = new Commit
                {
                    Message = "message",
                    Author = new Committer
                    {
                        Date = DateTime.Now,
                    }
                }
            });

            PullRequests = new ObservableCollection<PullRequest>();
            PullRequests.Add(new PullRequest
            {
                Title = "my pull requets",
                Body = "the description",
                UpdatedAt = DateTime.Now,
                State = "Open",
                User = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
            });

            Issues = new ObservableCollection<Issue>();
            Issues.Add(new Issue
            {
                Title = "my issue",
                Body = "the description is very long the description is very long the description is very long the description is very long the description is very long the description is very long the description is very long",
                UpdatedAt = DateTime.Now,
                State = "Open",
                User = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
            });
        }

        public Repository Repository { get; set; }

        public ObservableCollection<Push> Commits { get; set; }

        public ObservableCollection<PullRequest> PullRequests { get; set; }

        public ObservableCollection<Issue> Issues { get; set; }
    }
}