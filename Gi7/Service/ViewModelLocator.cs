using System;
using GalaSoft.MvvmLight;
using Gi7.Resources.DesignData;
using Gi7.Service.Navigation;
using Gi7.Views;
using Gi7.Controls;

namespace Gi7.Service
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

        public Object HomeViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return null;
                }
                else
                    return new HomeViewModel(GithubService, NavigationService);
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
                    return new RepositoryViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"));
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
                    return new UserViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"));
            }
        }

        public const string CommitUrl = "/Views/CommitView.xaml?user={0}&repo={1}&sha={2}";
        public Object CommitViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return new CommitDataModel();
                }
                else
                    return new CommitViewModel(GithubService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("sha"));
            }
        }
    }
}
