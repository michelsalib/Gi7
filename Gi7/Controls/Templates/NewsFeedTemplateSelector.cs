using System.Windows;
using Gi7.Client.Model.Event;

namespace Gi7.Controls.Templates
{
    public class NewsFeedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CommitCommentEventTemplate { get; set; }
        public DataTemplate CreateEventTemplate { get; set; }
        public DataTemplate DeleteEventTemplate { get; set; }
        public DataTemplate DownloadEventTemplate { get; set; }
        public DataTemplate FollowEventTemplate { get; set; }
        public DataTemplate ForkApplyEventTemplate { get; set; }
        public DataTemplate ForkEventTemplate { get; set; }
        public DataTemplate GistEventTemplate { get; set; }
        public DataTemplate IssueCommentEventTemplate { get; set; }
        public DataTemplate IssuesEventTemplate { get; set; }
        public DataTemplate MemberEventTemplate { get; set; }
        public DataTemplate PublicEventTemplate { get; set; }
        public DataTemplate PullRequestEventTemplate { get; set; }
        public DataTemplate PushEventTemplate { get; set; }
        public DataTemplate TeamAddEventTemplate { get; set; }
        public DataTemplate WatchEventTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is Event)
            {
                return this[item.GetType().Name + "Template"];
            }

            return base.SelectTemplate(item, container);
        }

        public DataTemplate this[string key]
        {
            get
            {
                return GetType().GetProperty(key).GetValue(this, null) as DataTemplate;
            }
        }
    }
}