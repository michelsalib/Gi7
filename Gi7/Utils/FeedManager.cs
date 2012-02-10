using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gi7.Client.Model.Feed.Base;
using System.Collections.Generic;
using Gi7.Client.Model.Feed;
using Gi7.Service;

namespace Gi7.Utils
{
    public class FeedManager
    {
        public void PopulateDestinationFormat(IEnumerable<Feed> feeds)
        {
            foreach (var feed in feeds)
            {
                if (feed is CommitCommentFeed || feed is PushFeed)
                {
                    feed.DestinationFormat = ViewModelLocator.CommitUrl;
                }
                else if (feed is RepositoryFeed) {
                    feed.DestinationFormat = ViewModelLocator.RepositoryUrl;
                }
                else if (feed is IssueCommentFeed || feed is IssueFeed) {
                    feed.DestinationFormat = ViewModelLocator.IssueUrl;
                }
                else {
                    feed.DestinationFormat = ViewModelLocator.UserUrl;
                }
            }
        }
    }
}
