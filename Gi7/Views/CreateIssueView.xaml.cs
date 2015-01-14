using System.Windows.Controls;

namespace Gi7.Views
{
    public partial class CreateIssueView
    {
        public CreateIssueView()
        {
            InitializeComponent();
        }

        private void TextKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            ((TextBox)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }
    }
}