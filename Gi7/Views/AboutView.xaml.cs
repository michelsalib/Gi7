using Gi7.ViewModel;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public partial class AboutView : PhoneApplicationPage
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void OnShare(object sender, System.EventArgs e)
        {
            var aboutViewModel = (AboutViewModel) DataContext;
            aboutViewModel.ShareCommand.Execute(null);
        }
    }
}
