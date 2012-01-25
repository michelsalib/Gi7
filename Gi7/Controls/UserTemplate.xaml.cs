using System;
using System.Windows.Controls;
using System.Windows.Input;
using Gi7.Model;
using Gi7.Service;

namespace Gi7.Controls
{
    public partial class UserTemplate : UserControl
    {
        public UserTemplate()
        {
            InitializeComponent();
        }

        private void StackPanel_Tap(object sender, GestureEventArgs e)
        {
            var user = DataContext as User;
            if (user != null)
                ViewModelLocator.NavigationService.NavigateTo(String.Format(ViewModelLocator.UserUrl, user.Login));
        }
    }
}