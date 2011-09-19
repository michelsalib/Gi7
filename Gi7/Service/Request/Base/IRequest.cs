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

namespace Gi7.Service.Request.Base
{
    public interface IRequest
    {
        String Uri { get; }

        OverrideSettings OverrideSettings { get; }
    }
}
