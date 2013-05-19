using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Gi7.Client.Request.Base;
using Gi7.Service;
using Gi7.ViewModel;

namespace Gi7.Controls.PaginatedListBox
{
    public class PaginatedListBox<TResult> : ListBox
        where TResult : class, new()
    {
        public static readonly DependencyProperty RequestProperty =
            DependencyProperty.Register("Request", typeof (IPaginatedRequest<TResult>), typeof (PaginatedListBox<TResult>), new PropertyMetadata(_newRequest));

        protected bool Loading;

        private bool _alreadyHookedScrollEvents;

        public PaginatedListBox()
        {
            Loaded += (s, e) =>
            {
                // prevent several hooking
                if (_alreadyHookedScrollEvents)
                    return;
                _alreadyHookedScrollEvents = true;

                // hook
                var sv = (ScrollViewer) FindElementRecursive(this, typeof (ScrollViewer));
                if (sv != null)
                {
                    var element = VisualTreeHelper.GetChild(sv, 0) as FrameworkElement;
                    if (element != null)
                    {
                        // get visual state
                        VisualStateGroup vgroup = FindVisualState(element, "VerticalCompression");
                        if (vgroup != null)
                            vgroup.CurrentStateChanging += (se, ev) =>
                            {
                                // on bottom compression, need to load next page
                                if (ev.NewState.Name == "CompressionBottom")
                                    _load();
                            };
                    }
                }
            };
        }

        public IPaginatedRequest<TResult> Request
        {
            get { return (IPaginatedRequest<TResult>) GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }

        private static void _newRequest(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (PaginatedListBox<TResult>) d;
            listBox.SetBinding(ItemsSourceProperty, new Binding("Request.Result") {RelativeSource = new RelativeSource(RelativeSourceMode.Self)});
            listBox._load();
        }

        private void _load()
        {
            try
            {
                if (Loading == false && Request.HasMoreItems)
                {
                    Loading = true;
                    ViewModelLocator.GithubService.Load(Request, r => Loading = false);
                }
            } catch (Exception)
            {
                ClearValue(ItemsSourceProperty);
            }
        }

        private UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                        return element as UIElement;
                    else
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                }
            return returnElement;
        }

        private VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(element);
            return groups.Cast<VisualStateGroup>().FirstOrDefault(@group => @group.Name == name);
        }
    }
}