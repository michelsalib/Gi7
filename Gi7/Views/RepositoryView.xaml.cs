using System.Linq;
using System.Windows.Controls;
using Gi7.ViewModel;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;
using System.Windows;

namespace Gi7.Views
{
    public partial class RepositoryView : PhoneApplicationPage
    {
        private PivotItem selectedItem;

        public RepositoryView()
        {
            InitializeComponent();
            Loaded += (sender, args) => { selectedItem = Pivot.SelectedItem as PivotItem; };
        }

        private void Pivot_Tap(object sender, GestureEventArgs e)
        {
            var currSelectedItem = Pivot.SelectedItem as PivotItem;
            if (currSelectedItem != null && selectedItem != null)
            {
                if (currSelectedItem.Name == selectedItem.Name)
                {
                    var listBox = currSelectedItem.Content as ListBox;
                    if (listBox != null)
                    {
                        object first = listBox.Items.FirstOrDefault();
                        if (first != null)
                            listBox.ScrollIntoView(first);
                    }
                }
                selectedItem = currSelectedItem;
            }
        }

        private void OpenContextMenu(object sender, GestureEventArgs e)
        {
            ContextMenu contextMenu = ContextMenuService.GetContextMenu((DependencyObject)sender);
            contextMenu.IsOpen = true;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            var repositoryViewModel = (RepositoryViewModel) DataContext;
            if (!repositoryViewModel.Loaded)
            {
                var url = e.Uri.ToString();
                var query = url.Substring(url.IndexOf('?')+1);
                var strings = query.Split('&');
                var dictionary = strings.ToDictionary(s => s.Split('=')[0], s => s.Split('=')[1]);
                repositoryViewModel.Load(dictionary["user"], dictionary["repo"]);
            }
            base.OnNavigatedTo(e);
        }
    }
}