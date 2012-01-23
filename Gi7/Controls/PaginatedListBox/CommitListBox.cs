using System;
using System.Collections.Generic;
using System.Linq;
using Gi7.Model;
using Gi7.Model.Extra;
using Gi7.Service;
using Gi7.Utils;

namespace Gi7.Controls.PaginatedListBox
{
    public class CommitListBox : PaginatedListBox<Push>
    {
        public CommitListBox()
        {
            useDefaultResult = false;
        }

        protected override void _load()
        {
            try
            {
                if (Loading == false && Request.HasMoreItems)
                {
                    Loading = true;
                    ViewModelLocator.GithubService.Load(Request, defaultAction: OnDefaultAction);
                }
            }
            catch (Exception)
            {
                ClearValue(ItemsSourceProperty);
            }
        }

        private void OnDefaultAction(List<Push> list, bool clear)
        {
            if (clear)
                Request.Result.Clear();

            if (list.Count < 30)
                Request.HasMoreItems = false;

            Request.Result.AddRange(list);

            Request.CustomResult = Request.Result.GroupBy(x => x.Commit.Author.Date.Trunk()).Select(x => new GroupPush(x)).ToObservableCollection();

            Loading = false;
        }
    }
}
