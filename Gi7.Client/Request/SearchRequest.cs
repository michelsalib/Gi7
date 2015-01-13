using System;
using System.Net;
using Gi7.Client.Model.Extra;
using Gi7.Client.Request.Base;

namespace Gi7.Client.Request
{
    public class SearchRequest : SingleRequest<SearchResult>
    {
        public SearchRequest(string query)
        {
            Uri = String.Format("/legacy/repos/search/{0}", HttpUtility.UrlEncode(query));
        }
    }
}