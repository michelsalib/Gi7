﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Gi7.Client.Model.Extra;
using RestSharp;
using RestSharp.Deserializers;

namespace Gi7.Client.Utils
{
    public class FeaturedRepoDeserializer : IDeserializer
    {
        #region IDeserializer Members

        public string DateFormat { get; set; }

        public string RootElement { get; set; }

        public string Namespace { get; set; }

        public T Deserialize<T>(IRestResponse response)
        {
            var result = Activator.CreateInstance<T>();
            var repos = result as List<FeaturedRepo>;

            if (repos == null)
                throw new InvalidOperationException("The type must be List<FeaturedRepo>.");

            repos.AddRange(from item in XDocument.Parse(response.Content).Descendants("item")
                           let description = item.Element("description").Value
                           let match = new Regex("https://github\\.com/([^/]*)/([^\"]*)").Match(description)
                           where match.Success
                           select new FeaturedRepo
                           {
                               Title = item.Element("title").Value, User = match.Groups[1].Value, Repo = match.Groups[2].Value,
                           });

            return result;
        }

        #endregion
    }
}