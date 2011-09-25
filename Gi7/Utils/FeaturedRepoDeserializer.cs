using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Gi7.Model.Extra;
using RestSharp.Deserializers;

namespace Gi7.Utils
{
    public class FeaturedRepoDeserializer : IDeserializer
    {
        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public T Deserialize<T>(RestSharp.RestResponse response) where T : new()
        {
            T result = new T();
            var repos = result as List<FeaturedRepo>;

            if (repos == null)
                throw new InvalidOperationException("The type must be List<FeaturedRepo>.");

            foreach (var item in XDocument.Parse(response.Content).Descendants("item"))
            {
                var description = item.Element("description").Value;
                var match = new Regex("https://github\\.com/([^/]*)/([^\"]*)").Match(description);
                if (match.Success)
                {
                    repos.Add(new FeaturedRepo()
                    {
                        Title = item.Element("title").Value,
                        User = match.Groups[1].Value,
                        Repo = match.Groups[2].Value,
                    });
                }
            }

            return result;
        }
    }
}
