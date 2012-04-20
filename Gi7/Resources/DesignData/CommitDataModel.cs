using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Gi7.Client.Model;
using Gi7.Utils.ViewModels;
using System.Windows.Media;

namespace Gi7.Resources.DesignData
{
    public class CommitDataModel
    {
        public CommitDataModel()
        {
            RepoName = "michelsalib/Gi7";

            Commit = new Push
            {
                Author = new User
                {
                    Login = "michelsalib",
                    AvatarUrl = "https://secure.gravatar.com/avatar/5c4663b12b9e6d8dc6dcdfbfb3dc1317",
                },
                Commit = new Commit
                {
                    Message = "-- general performance improvements\n  -- removed most of the sub view models\n  -- support late loading of the panorama/pivot items",
                    Author = new Committer
                    {
                        Date = DateTime.Now,
                    }
                },
                Stats = new PushStats
                {
                    Additions = 468,
                    Deletions = 352,
                    Total = 820
                },
            };


            Files = new ObservableCollection<CommitFile>
            {
                new CommitFile
                {
                    File = new File() {
                        Filename = "Fie.cs",
                    },
                    Lines = new ObservableCollection<CommitLine> {
                        new CommitLine() {
                            Color = new SolidColorBrush(Colors.Blue),
                            Line = "@@ line description",
                        },
                        new CommitLine() {
                            Color = new SolidColorBrush(Colors.Green),
                            Line = "+ new line",
                        },
                        new CommitLine() {
                            Color = new SolidColorBrush(Colors.Red),
                            Line = "- old line",
                        },
                        new CommitLine() {
                            Color = new SolidColorBrush(Colors.White),
                            Line = "single line",
                        }
                    },
                }
            };

            CommentsRequest = new StubPaginatedRequest<Comment>()
            {
                Result = new List<Comment>()
                {
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

        public String RepoName { get; set; }

        public Push Commit { get; set; }

        public ObservableCollection<CommitFile> Files {get;set;}

        public StubPaginatedRequest<Comment> CommentsRequest { get; set; }

        public String CommitText
        {
            get { return String.Format("Showing {0} changed files with {1} additions and {2} deletions.", Commit.Files.Count, Commit.Stats.Additions, Commit.Stats.Deletions); }
        }
    }
}