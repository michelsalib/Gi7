using System;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using System.Windows.Input;

namespace Gi7.Views
{
    public partial class HomeView : PhoneApplicationPage
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }
    }
}