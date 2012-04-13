using Microsoft.Phone.Controls;
using System.Windows.Controls;

namespace Gi7.Views
{
    public partial class PullRequestView : PhoneApplicationPage
    {
        public PullRequestView()
        {
            InitializeComponent();
        }

        private void CommentKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}