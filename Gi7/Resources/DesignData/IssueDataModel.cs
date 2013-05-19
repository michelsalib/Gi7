using System;
using System.Collections.Generic;
using Gi7.Client.Model;

namespace Gi7.Resources.DesignData
{
    public class IssueDataModel
    {
        public IssueDataModel()
        {
            Issue = new Issue
            {
                Body = "This is a very important issue, nothing works! This is a very important issue, nothing works! This is a very important issue, nothing works! This is a very important issue, nothing works! This is a very important issue, nothing works! This is a very important issue, nothing works!",
                Title = "Nothing works",
                User = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
            };

            CommentsRequest = new StubPaginatedRequest<Comment>
            {
                Result = new List<Comment>
                {
                    new Comment
                    {
                        Body = "My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice ! My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
                    {
                        Body = "My comment is nice !",
                        User = new User
                        {
                            Login = "michelsalib",
                            AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                        },
                        UpdatedAt = DateTime.Now,
                    },
                    new Comment
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

        public Issue Issue { get; set; }

        public StubPaginatedRequest<Comment> CommentsRequest { get; set; }
    }
}