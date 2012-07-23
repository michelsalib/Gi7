using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Gi7.Client;
using Gi7.Client.Model;
using Gi7.Client.Request.Repository;
using Gi7.Service;
using Gi7.Service.Navigation;
using UserRequest = Gi7.Client.Request.User;
using Microsoft.Phone.Tasks;
using Gi7.Client.Request.Issue;

namespace Gi7.Views
{
    public class CreateIssueViewModel : ViewModelBase
    {
        public RelayCommand ShareCommand { get; private set; }
        public RelayCommand<User> UserSelectedCommand { get; private set; }
        public RelayCommand<Repository> RepoSelectedCommand { get; private set; }
        public RelayCommand<Repository> CreateIssueCommand {get; private set;}

        public CreateIssueViewModel(GithubService githubService, INavigationService navigationService, string user, string repo)
        {
            //Michelsalib = githubService.Load(new UserRequest.Get("michelsalib"), u => Michelsalib = u);
            //AlbertoMonteiro = githubService.Load(new UserRequest.Get("albertomonteiro"), u => AlbertoMonteiro = u);
            //Gi7 = githubService.Load(new Get("michelsalib", "Gi7"), r => Gi7 = r);

            //RepoSelectedCommand = new RelayCommand<Repository>(r =>
            //{
            //    if (r != null)
            //        navigationService.NavigateTo(String.Format(ViewModelLocator.RepositoryUrl, r.Owner.Login, r.Name));
            //});
            //UserSelectedCommand = new RelayCommand<User>(user =>
            //{
            //    if (user != null)
            //        navigationService.NavigateTo(string.Format(ViewModelLocator.UserUrl, user.Login));
            //});
            //ShareCommand = new RelayCommand(() =>
            //{
            //    new ShareLinkTask()
            //    {
            //        LinkUri = new Uri("http://www.windowsphone.com/en-US/apps/2bdbe5da-a20a-42f5-8b08-cda2fbf9046f"),
            //        Title = "Check this Github app for Windows Phone 7",
            //        Message = "I found this app that you might like. Check it ou on the Marketplace, it is free!",
            //    }.Show();
            //});

            CreateIssueCommand = new RelayCommand<Repository>((repo) => {
                githubService.Load( //... I'm stuck. What do I write here? Are those parameters even right? And where did I get them?
                });
            }, () => IsWatching.HasValue && !IsWatching.Value);
        }        


    }
}