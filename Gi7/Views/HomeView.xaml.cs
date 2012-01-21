using System;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace Gi7.Views
{
    public partial class HomeView : PhoneApplicationPage
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void Logout(object sender, EventArgs e)
        {
            Messenger.Default.Send(true, "logout");
        }

        private void About(object sender, EventArgs e)
        {
            Messenger.Default.Send(true, "about");
        }
    }
}