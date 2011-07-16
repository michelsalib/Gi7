using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Github7.Service
{
    public class LoadingEventArgs : EventArgs
    {
        public bool IsLoading { get; private set; }

        public LoadingEventArgs(bool isLoading)
        {
            IsLoading = isLoading;
        }
    }
}
