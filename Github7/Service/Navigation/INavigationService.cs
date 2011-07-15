using System;
using System.Windows.Navigation;

namespace Github7.Service.Navigation
{
    public interface INavigationService
    {
        void GoBack();
        void NavigateTo(String pageUri);
        event NavigatingCancelEventHandler Navigating;

        string GetParameter(string key, string defaultValue = "");
    }
}
