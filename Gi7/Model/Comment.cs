using System;

namespace Gi7.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public String Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Line { get; set; }

        public User User { get; set; }

        public DateTime UpdatedAt { get; set; }

        public String Url { get; set; }

        public String CommitId { get; set; }
    }
}