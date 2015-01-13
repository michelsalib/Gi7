using Octokit;

namespace Gi7.Client.Request.Issue
{
    public class IssueFunctions
    {
        /// <summary>
        /// Creates a new issue with the passed user in the passed repo
        /// </summary>
        /// <param name="gitConnection">Git Connection. Is genereated in the GithubService class</param>
        /// <param name="title">Title of the Issue. This is mandatory</param>
        /// <param name="body">Description of the issue</param>
        /// <param name="repo">Name of the repository</param>
        /// <param name="username">usename of the sending user. mail address won't work</param>
        public static void CreateIssue(ApiConnection gitConnection, string title,  string repo, string username, string body = "")
        {
            if (gitConnection == null) return;

            var issue = new NewIssue(title) {Body = body};

            var client = new IssuesClient(gitConnection);
            client.Create(username, repo, issue);
        }
    }
}
