using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Gi7.Client;
using Gi7.Controls;
using Gi7.Resources.DesignData;
using Gi7.Service.Navigation;
using Gi7.Utils;
using Microsoft.Practices.ServiceLocation;

namespace Gi7.ViewModel
{
    public class ViewModelLocator
    {
        public const string HOME_URL = "/Views/HomeView.xaml";
        public const string REPOSITORY_URL = "/Views/RepositoryView.xaml?user={0}&repo={1}";
        public const string USER_URL = "/Views/UserView.xaml?user={0}";
        public const string COMMIT_URL = "/Views/CommitView.xaml?user={0}&repo={1}&sha={2}";
        public const string PULL_REQUEST_URL = "/Views/PullRequestView.xaml?user={0}&repo={1}&number={2}";
        public const string ISSUE_URL = "/Views/IssueView.xaml?user={0}&repo={1}&number={2}";
        public const string TREE_URL = "/Views/TreeView.xaml?user={0}&repo={1}&sha={2}&path={3}";
        public const string BLOB_URL = "/Views/BlobView.xaml?user={0}&repo={1}&sha={2}&path={3}";
        public const string ABOUT_URL = "/Views/AboutView.xaml";
        public const string CREATE_ISSUE_URL = "/Views/CreateIssueView.xaml?user={0}&repo={1}";

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {

            }
            else
            {
                NavigationService = new NavigationService();
                GithubService = new GithubService();
                GithubService.IsAuthenticatedChanged += (s, e) =>
                {
                    if (e.IsAuthenticated == false && !NavigationService.CurrentUri().Contains(HOME_URL))
                        NavigationService.NavigateTo(HOME_URL);
                };
                GithubService.Loading += (s, e) => { GlobalLoading.Instance.IsLoading = e.IsLoading; };
                GithubService.ConnectionError += (s, e) => MessageBox.Show("Server unreachable.", "Gi7", MessageBoxButton.OK);
                GithubService.Unauthorized += (s, e) => MessageBox.Show("Wrong credentials.", "Gi7", MessageBoxButton.OK);
                GithubService.Init();
            }
        }

        public static GithubService GithubService { get; private set; }

        public static INavigationService NavigationService { get; private set; }

        public LoginPanelViewModel LoginPanelViewModel
        {
            get { return new LoginPanelViewModel(GithubService); }
        }

        public HomeViewModel HomeViewModel
        {
            get
            {
                return new HomeViewModel(GithubService, NavigationService);
            }
        }

        public Object RepositoryViewModel
        {
            get { return ViewModelBase.IsInDesignModeStatic ? (object)new RepositoryDataModel() : new RepositoryViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo")); }
        }

        public Object CreateIssueViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return null;
                else
                    return new CreateIssueViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"));
            }
        }

        public Object UserViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new UserDataModel();
                else
                    return new UserViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"));
            }
        }

        public Object CommitViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new CommitDataModel();
                else
                    return new CommitViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("sha"));
            }
        }

        public Object TreeViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new TreeDataModel();
                else
                    return new TreeViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("sha"), NavigationService.GetParameter("path"));
            }
        }

        public Object BlobViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new BlobDataModel();
                else
                    return new BlobViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("sha"), NavigationService.GetParameter("path"));
            }
        }

        public Object PullRequestViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new PullRequestDataModel();
                else
                    return new PullRequestViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("number"));
            }
        }

        public Object IssueViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new IssueDataModel();
                else
                    return new IssueViewModel(GithubService, NavigationService, NavigationService.GetParameter("user"), NavigationService.GetParameter("repo"), NavigationService.GetParameter("number"));
            }
        }

        public Object AboutViewModel
        {
            get
            {
                if (ViewModelBase.IsInDesignModeStatic)
                    return new AboutDataModel();
                else
                    return new AboutViewModel(GithubService, NavigationService);
            }
        }
    }
}