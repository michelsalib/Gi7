using Octokit;

namespace Gi7.Client.Request.Issue
{
    public class IssueFunctions
    {
        /// <summary>
        /// Creates a new issue with the passed user in the passed repo
        /// </summary>
        /// <param name="gitHubClient">GitHub Client. Is genereated in the GithubService class</param>
        /// <param name="title">Title of the Issue. This is mandatory</param>
        /// <param name="body">Description of the issue</param>
        /// <param name="repo">Name of the repository</param>
        /// <param name="username">usename of the sending user. mail address won't work</param>
        public static void CreateIssue(IGitHubClient gitHubClient, string title, string repo, string username, string body = "")
        {
            if (gitHubClient == null) return;

            gitHubClient.Issue.Create(username, repo, new NewIssue(title) { Body = body });
        }
    }
}
