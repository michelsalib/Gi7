using Newtonsoft.Json;

namespace Gi7.Client.Model
{
    public class Organization
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("login")]
        public string Login { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("avatar_url")]
        public string Avatar { get; set; }
    }
}