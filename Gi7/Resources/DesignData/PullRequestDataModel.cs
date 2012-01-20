using Gi7.Model;

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
        }

        public PullRequest PullRequest { get; set; }
    }
}