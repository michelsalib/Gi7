using System;
using GalaSoft.MvvmLight;
using Gi7.Controls;
using Gi7.Resources.DesignData;
using Gi7.Service.Navigation;
using Gi7.Views;

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
                NavigationService = new NavigationService();
                GithubService = new GithubService();
                GithubService.IsAuthenticatedChanged += (s,e) => {
                    if(e.IsAuthenticated == false && !NavigationService.CurrentUri().Contains(HomeUrl)){
                        NavigationService.NavigateTo(ViewModelLocator.HomeUrl);
                    }
                };
            }
        }

        public LoginPanelViewModel LoginPanelViewModel
        {
            get
            {
                return new LoginPanelViewModel(GithubService);
            }
        }

        public const string HomeUrl = "/Views/HomeView.xaml";
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

        public const string PullRequestUrl = "/Views/PullRequestView.xaml?user={0}&repo={1}&number={2}";
        public Object PullRequestViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return new PullRequestDataModel();
                }
                else
                    return new PullRequestViewModel(GithubService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("number"));
            }
        }

        public const string IssueUrl = "/Views/IssueView.xaml?user={0}&repo={1}&number={2}";
        public Object IssueViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return new IssueDataModel();
                }
                else
                    return new IssueViewModel(GithubService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("number"));
            }
        }

        public const string AboutUrl = "/Views/AboutView.xaml";
        public Object AboutViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                {
                    return new AboutDataModel();
                }
                else
                    return new AboutViewModel(GithubService);
            }
        }
    }
}
