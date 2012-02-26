using System;
using Newtonsoft.Json;

namespace Gi7.Client.Model
{
    public class Comment : BoolModel
    {
        public int Id { get; set; }

        public String Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? Line { get; set; }

        public User User { get; set; }

        public DateTime UpdatedAt { get; set; }

        public String Url { get; set; }

        [JsonProperty("commit_id")]
        public String CommitId { get; set; }
    }
}