using System;
using System.Linq;
using Newtonsoft.Json;

namespace Gi7.Client.Model
{
    public class User : BoolModel
    {
        private string _avatarUrl;
        public string Login { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        [JsonProperty("created_at")]
        public DateTime Joined { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Blog { get; set; }
        public string Bio { get; set; }
        public string HtmlUrl { get; set; }
        public string Url { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        [JsonProperty("public_repos")]
        public int PublicRepos { get; set; }
        public int TotalPrivateRepos { get; set; }

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set
            {
                // trim GET parameters
                _avatarUrl = new string(value.TakeWhile(c => c != '?').ToArray());
            }
        }

        [JsonProperty("gravatar_id")]
        public string GravatarId
        {
            set
            {
                _avatarUrl = string.Format("https://secure.gravatar.com/avatar/{0}", value);
            }
        }
    }
}