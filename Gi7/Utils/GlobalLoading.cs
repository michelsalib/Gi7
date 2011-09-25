using System.Windows.Navigation;
// using AgFx;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Gi7.Utils
{
    /// <summary>
    /// <see cref="http://www.jeff.wilcox.name/2011/07/creating-a-global-progressindicator-experience-using-the-windows-phone-7-1-sdk-beta-2/"/>
    /// </summary>
    public class GlobalLoading
    {
        private ProgressIndicator _mangoIndicator;

        private GlobalLoading()
        {
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
            var ee = e.Content;
            var pp = ee as PhoneApplicationPage;
            if (pp != null)
            {
                pp.SetValue(SystemTray.ProgressIndicatorProperty, _mangoIndicator);
            }
        }

        private static GlobalLoading _in;
        public static GlobalLoading Instance
        {
            get
            {
                if (_in == null)
                {
                    _in = new GlobalLoading();
                }

                return _in;
            }
        }

        private int _loadingCount;

        public bool IsLoading
        {
            get
            {
                return _loadingCount > 0;
            }
            set
            {
                if (value)
                {
                    ++_loadingCount;
                }
                else
                {
                    --_loadingCount;
                }

                NotifyValueChanged();
            }
        }

        private void NotifyValueChanged()
        {
            if (_mangoIndicator != null)
            {
                _mangoIndicator.IsIndeterminate = _loadingCount > 0;

                if (_mangoIndicator.IsVisible == false)
                {
                    _mangoIndicator.IsVisible = true;
                }
            }
        }
    }
}
