using System;
using GalaSoft.MvvmLight;
using Github7.Resources.DesignData;
using Github7.Service.Navigation;
using Github7.Views;
using Github7.Controls;

namespace Github7.Service
{
    public class ViewModelLocator
    {
        public static GithubService GithubService {get;private set;}

        public static INavigationService NavigationService { get; private set; }

        static ViewModelLocator()
        {
            if (!ViewModelBase.IsInDesignModeStatic)
            {
                GithubService = new GithubService();
                NavigationService = new NavigationService();
            }
        }

        public LoginPanelViewModel LoginPanelViewModel
        {
            get
            {
                return new LoginPanelViewModel(GithubService);
            }
        }

        public FeedsPanelViewModel FeedsPanelViewModel
        {
            get
            {
                return new FeedsPanelViewModel(GithubService, NavigationService);
            }
        }

        public UserPanelViewModel UserPanelViewModel
        {
            get
            {
                return new UserPanelViewModel(GithubService, NavigationService.GetParameter("user", GithubService.Username));
            }
        }

        public RepositoryPanelViewModel RepositoryPanelViewModel
        {
            get
            {
                return new RepositoryPanelViewModel(GithubService, NavigationService, NavigationService.GetParameter("user", GithubService.Username));
            }
        }

        public UsersPanelViewModel UsersPanelViewModel
        {
            get
            {
                return new UsersPanelViewModel(GithubService, NavigationService, NavigationService.GetParameter("user", GithubService.Username));
            }
        }

        public AboutPanelViewModel AboutPanelViewModel
        {
            get
            {
                return new AboutPanelViewModel(GithubService, NavigationService);
            }
        }

        public const string HomeUrl = "/Views/HomeView.xaml";
        public HomeViewModel HomeViewModel
        {
            get
            {
                return new HomeViewModel(GithubService);
            }
        }

        public const string RepositoryUrl = "/Views/RepositoryView.xaml?user={0}&repo={1}";
        public Object RepositoryViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return new RepositoryDataModel();
                }
                else
                    return new RepositoryViewModel(GithubService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"));
            }
        }

        public const string UserUrl = "/Views/UserView.xaml?user={0}";
        public Object UserViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return new UserDataModel();
                }
                else
                    return new UserViewModel(GithubService);
            }
        }
    }
}
