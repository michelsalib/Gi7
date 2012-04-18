using System;
using System.Windows.Media;
using GalaSoft.MvvmLight;

namespace Gi7.Utils.ViewModels
{
    public class CommitLine : ViewModelBase
    {
        private String _line;
        public String Line
        {
            get { return _line; }
            set { _line = value; }
        }

        private SolidColorBrush _color;
        public SolidColorBrush Color
        {
            get { return _color; }
            set { _color = value; }
        }
    }
}
