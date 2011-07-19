using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using Gi7.Utils.Messages;
using Gi7.Service;

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
            Messenger.Default.Send<bool>(true, "logout");
        }
    }
}