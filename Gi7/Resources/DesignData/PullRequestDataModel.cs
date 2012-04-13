using Gi7.Client.Model;
using System.Collections.Generic;
using System;

namespace Gi7.Resources.DesignData
{
    public class PullRequestDataModel
    {
        public PullRequestDataModel()
        {
            PullRequest = new PullRequest
            {
                User = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
                Body = "Look at my nice PR!",
                Additions = 468,
                Deletions = 352,
                Commits = 8,
                State = "Open"
            };


            CommentsRequest = new StubPaginatedRequest<Comment>()
            {
                Result = new List<Comment>()
                {
                    new Comment()
                    {
                        Body = "My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment()
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    }
                }
            };
        }

        public PullRequest PullRequest { get; set; }

        public StubPaginatedRequest<Comment> CommentsRequest { get; set; }
    }
}