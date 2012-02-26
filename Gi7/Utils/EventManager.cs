using System.Collections.Generic;
using Gi7.Client.Model.Event;
using System;
using Gi7.Service;

namespace Gi7.Utils
{
    public class EventManager
    {
        public String GetDestination(Event e)
        {
            if (e is CommitCommentEvent)
            {
                return String.Format(ViewModelLocator.CommitUrl, e.Repo.Owner.Login, e.Repo.Name, ((CommitCommentEvent)e).Comment.CommitId);
            }
            else if (e is PushEvent)
            {
                return String.Format(ViewModelLocator.CommitUrl, e.Repo.Owner.Login, e.Repo.Name, ((PushEvent)e).Head);
            }
            else if (e is IssuesEvent)
            {
                return String.Format(ViewModelLocator.IssueUrl, e.Repo.Owner.Login, e.Repo.Name, ((IssuesEvent)e).Issue.Number);
            }
            else if (e is IssueCommentEvent)
            {
                return String.Format(ViewModelLocator.IssueUrl, e.Repo.Owner.Login, e.Repo.Name, ((IssueCommentEvent)e).Issue.Number);
            }

            return String.Format(ViewModelLocator.RepositoryUrl, e.Repo.Owner.Login, e.Repo.Name); 
        }

        public String GetDescription(Event e)
        {
            if (e is CommitCommentEvent)
            {
                return "commented commit on";
            }
            else if (e is CreateEvent)
            {
                return String.Format("created branch {0} at", ((CreateEvent)e).Ref);
            }
            else if (e is DeleteEvent)
            {
                return String.Format("deleted branch {0} at", ((DeleteEvent)e).Ref);
            }
            else if (e is FollowEvent)
            {
                return String.Format("started following {0}", ((FollowEvent)e).Target.Login);
            }
            else if (e is ForkEvent)
            {
                return "forked";
            }
            else if (e is IssueCommentEvent)
            {
                return "commented issue on";
            }
            else if (e is IssuesEvent)
            {
                return String.Format("{0} issue on", ((IssuesEvent)e).Action);
            }
            else if (e is PullRequestEvent)
            {
                return String.Format("{0} pull request on", ((PullRequestEvent)e).Action);
            }
            else if (e is PushEvent)
            {
                return "pushed on";
            }
            else if (e is WatchEvent)
            {
                return String.Format("{0} watching on", ((WatchEvent)e).Action);
            }

            return "did a non supported action at";
        }

        public String GetImage(Event e)
        {
            if (e is CommitCommentEvent)
            {
                return "/Gi7;component/Images/comment.png";
            }
            else if (e is CreateEvent)
            {
                return "/Gi7;component/Images/create.png";
            }
            else if (e is DeleteEvent)
            {
                return "/Gi7;component/Images/delete.png";
            }
            else if (e is FollowEvent)
            {
                return "/Gi7;component/Images/follow.png";
            }
            else if (e is ForkEvent)
            {
                return "/Gi7;component/Images/fork.png";
            }
            else if (e is IssueCommentEvent)
            {
                return "/Gi7;component/Images/issues_comment.png";
            }
            else if (e is IssuesEvent)
            {
                return String.Format("/Gi7;component/Images/issues_{0}.png", ((IssuesEvent)e).Action);
            }
            else if (e is PullRequestEvent)
            {
                return "/Gi7;component/Images/issues_opened.png";
            }
            else if (e is PushEvent)
            {
                return "/Gi7;component/Images/push.png";
            }

            return null;
        }
    }
}
