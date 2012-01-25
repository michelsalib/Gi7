using System.Linq;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

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
    }
}