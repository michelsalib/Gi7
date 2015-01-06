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

        [JsonProperty("avatar_url")]
        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set
            {
                // Used to determine if there is something with wrong with the link used to get the users avatar
                //_avatarUrl = string.Format("https://avatars.githubusercontent.com/u/{0}", value); 

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