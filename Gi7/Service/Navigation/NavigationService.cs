using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Gi7.Service.Navigation
{
    public class NavigationService : INavigationService
    {
        private Dictionary<String, String> _currentQueryString;

        private PhoneApplicationFrame _mainFrame;

        public event NavigatingCancelEventHandler Navigating;

        public void NavigateTo(string pageUri)
        {
            if (EnsureMainFrame())
            {
                _mainFrame.Navigate(new Uri(pageUri, UriKind.RelativeOrAbsolute));
                _currentQueryString = pageUri.Substring(pageUri.IndexOf('?') + 1).Split('&').Select(i =>
                {
                    var values = i.Split('=');
                    return new KeyValuePair<String, String>(values[0], values[1]);
                }).ToDictionary(i => i.Key, i => i.Value);
            }
        }

        public void GoBack()
        {
            if (EnsureMainFrame()
                && _mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
            }
        }

        private bool EnsureMainFrame()
        {
            if (_mainFrame != null)
            {
                return true;
            }

            _mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;

            if (_mainFrame != null)
            {
                // Could be null if the app runs inside a design tool
                _mainFrame.Navigating += (s, e) =>
                {
                    if (Navigating != null)
                    {
                        Navigating(s, e);
                    }
                };

                return true;
            }

            return false;
        }


        public string GetParameter(string key, string defaultValue = "")
        {
            var result = defaultValue;

            if (_currentQueryString != null && _currentQueryString.ContainsKey(key))
            {
                result = _currentQueryString[key];
            }

            return result;
        }
    }
}
