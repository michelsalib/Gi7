using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

// using AgFx;

namespace Gi7.Utils
{
    /// <summary>
    /// <see cref="http://www.jeff.wilcox.name/2011/07/creating-a-global-progressindicator-experience-using-the-windows-phone-7-1-sdk-beta-2/"/>
    /// </summary>
    public class GlobalLoading
    {
        private static GlobalLoading _in;
        private int _loadingCount;
        private ProgressIndicator _mangoIndicator;

        private GlobalLoading() {}

        public static GlobalLoading Instance
        {
            get
            {
                if (_in == null)
                    _in = new GlobalLoading();

                return _in;
            }
        }

        public bool IsLoading
        {
            get { return _loadingCount > 0; }
            set
            {
                if (value)
                    ++_loadingCount;
                else
                    --_loadingCount;

                NotifyValueChanged();
            }
        }

        public void Initialize(PhoneApplicationFrame frame)
        {
            _mangoIndicator = new ProgressIndicator();

            frame.Navigated += OnRootFrameNavigated;

            (frame.Content as PhoneApplicationPage).SetValue(SystemTray.ProgressIndicatorProperty, _mangoIndicator);
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            // Use in Mango to share a single progress indicator instance.
            object ee = e.Content;
            var pp = ee as PhoneApplicationPage;
            if (pp != null)
                pp.SetValue(SystemTray.ProgressIndicatorProperty, _mangoIndicator);
        }

        private void NotifyValueChanged()
        {
            if (_mangoIndicator != null)
            {
                _mangoIndicator.IsIndeterminate = _loadingCount > 0;

                if (_mangoIndicator.IsVisible == false)
                    _mangoIndicator.IsVisible = true;
            }
        }
    }
}