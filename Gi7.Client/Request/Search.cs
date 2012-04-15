using Gi7.Client.Model.Extra;
using Gi7.Client.Request.Base;
using System;
using System.Net;
using RestSharp;
using HtmlAgilityPack;
using System.Collections.ObjectModel;

namespace Gi7.Client.Request
{
    public class Search : SingleRequest<ObservableCollection<SearchResult>>
    {
        public Search(string query)
        {
            Uri = String.Format("/search?q={0}", HttpUtility.UrlEncode(query));
        }

        public override void Execute(RestSharp.RestClient client, Action<ObservableCollection<SearchResult>> callback = null)
        {
            var request = new RestRequest(Uri);

            client.BaseUrl = "https://github.com";

            RaiseLoading(true);

            client.ExecuteAsync(request, r =>
            {
                RaiseLoading(false);

                if (r.StatusCode == HttpStatusCode.Unauthorized)
                {
                    RaiseUnauthorized();
                }
                else if (r.ResponseStatus == ResponseStatus.Error)
                {
                    RaiseConnectionError();
                }
                else
                {
                    var html = new HtmlDocument();
                    html.LoadHtml(r.Content);
                    var repos = html.DocumentNode.SelectNodes("//table//td[1]/div[@class=\"result\"]/h2/a");
                    var users = html.DocumentNode.SelectNodes("//table//td[2]/div[@class=\"result\"]/h2/a");

                    var data = new ObservableCollection<SearchResult>();

                    // add repos
                    if (repos != null)
                    {
                        foreach (var item in repos)
                        {
                            data.Add(new SearchResult()
                            {
                                Name = item.InnerText,
                                Type = "repo",
                            });
                        }
                    }
                    // add users
                    if (users != null)
                    {
                        foreach (var item in users)
                        {
                            data.Add(new SearchResult()
                            {
                                Name = item.InnerText,
                                Type = "user",
                            });
                        }
                    }

                    Result = data;

                    RaiseSuccess(data);

                    if (callback != null)
                    {
                        callback(data);
                    }
                }
            });
        }
    }
}
