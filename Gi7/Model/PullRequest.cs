using System;

namespace Gi7.Model
{
    public class PullRequest : BoolModel
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public String Title { get; set; }

        public int Additions { get; set; }

        public int IssueId { get; set; }

        public int Commits { get; set; }

        public int Deletions { get; set; }

        public String Url { get; set; }

        public User User { get; set; }

        public String IssueUrl { get; set; }

        public String DiffUrl { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public String State { get; set; }

        public String HtmlUrl { get; set; }

        public DateTime ClosedAt { get; set; }

        public String PatchUrl { get; set; }

        public String Body { get; set; }

        public DateTime MergedAt { get; set; }
    }
}