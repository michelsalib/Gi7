using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Gi7.Service;
using Gi7.Service.Request.Base;

namespace Gi7.Controls.PaginatedListBox
{
    public class PaginatedListBox<TSource, TDestination> : ListBox
        where TSource : class, new()
        where TDestination : class, new()
    {
        public IPaginatedRequest<TSource, TDestination> Request
        {
            get { return (IPaginatedRequest<TSource, TDestination>)GetValue(RequestProperty); }
            set { SetValue(RequestProperty, value); }
        }
        public static readonly DependencyProperty RequestProperty =
            DependencyProperty.Register("Request", typeof(IPaginatedRequest<TSource, TDestination>), typeof(PaginatedListBox<TSource, TDestination>), new PropertyMetadata(_newRequest));

        protected bool Loading = false;

        private bool _alreadyHookedScrollEvents = false;

        public PaginatedListBox()
        {
            Loaded += (s, e) =>
            {
                // prevent several hooking
                if (_alreadyHookedScrollEvents)
                    return;
                _alreadyHookedScrollEvents = true;

                // hook
                var sv = (ScrollViewer)FindElementRecursive(this, typeof(ScrollViewer));
                if (sv != null)
                {
                    FrameworkElement element = VisualTreeHelper.GetChild(sv, 0) as FrameworkElement;
                    if (element != null)
                    {
                        // get visual state
                        VisualStateGroup vgroup = FindVisualState(element, "VerticalCompression");
                        if (vgroup != null)
                        {
                            vgroup.CurrentStateChanging += (se, ev) =>
                            {
                                // on bottom compression, need to load next page
                                if (ev.NewState.Name == "CompressionBottom")
                                {
                                    _load();
                                }
                            };
                        }
                    }
                }      
            };
        }

        private static void _newRequest(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (PaginatedListBox<TSource, TDestination>)d;
            listBox.SetBinding(ListBox.ItemsSourceProperty, new Binding("Request.Result") { RelativeSource = new RelativeSource(RelativeSourceMode.Self) });
            listBox._load();
        }

        private void _load()
        {
            try
            {
                if (Loading == false && Request.HasMoreItems)
                {
                    Loading = true;
                    ViewModelLocator.GithubService.Load<TSource, TDestination>(Request, r => Loading = false);
                }
            }
            catch (Exception)
            {
                ClearValue(ItemsSourceProperty);
            }
        }

        private UIElement FindElementRecursive(FrameworkElement parent, Type targetType)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            UIElement returnElement = null;
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    Object element = VisualTreeHelper.GetChild(parent, i);
                    if (element.GetType() == targetType)
                    {
                        return element as UIElement;
                    }
                    else
                    {
                        returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType);
                    }
                }
            }
            return returnElement;
        }
        private VisualStateGroup FindVisualState(FrameworkElement element, string name)
        {
            if (element == null)
                return null;

            IList groups = VisualStateManager.GetVisualStateGroups(element);
            foreach (VisualStateGroup group in groups)
                if (group.Name == name)
                    return group;

            return null;
        }
    }
}
