using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using GithubRepoExample.Interfaces;
using GithubRepoExample.Models;
using GithubRepoExample.Views;
using Octokit;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace GithubRepoExample.ViewModels
{
    public class ItemDetailsViewModel : ViewModelBase
    {
        private int Increment { get; set; }
        private int PageSize { get; set; }
        private int Skip { get; set; }
        private IGithubService GitHub { get; set; }

        public ObservableCollection<PullRequest> MasterPullRequests { get; set; }
        public ObservableCollection<PullRequest> PullRequests { get; set; }

        public string OpenLabel { get; set; }
        public string ClosedLabel { get; set; }

        public ICommand GoToItemCommand { get { return new Command(GoToItem); } }
        public ICommand LoadMoreCommand { get { return new Command(GetItems); } }


        public ItemDetailsViewModel(IAppNavigationService navigationService, IPageDialogService pageDialogService,
                                 IDeviceService deviceService, IGithubService gitService)
            : base(navigationService, pageDialogService, deviceService)
        {
            GitHub = gitService;
            Increment = 1;
            PageSize = 50;
            Skip = 0;
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                if (parameters.ContainsKey("PRID"))
                {
                    long prid = 0;
                    var id = parameters.GetValue<string>("PRID");
                    long.TryParse(id, out prid);
                    if (prid != 0)
                    {
                        MasterPullRequests = new ObservableCollection<PullRequest>(await GitHub.GetRepository(prid));
                        PullRequests = new ObservableCollection<PullRequest>();
                        GetItems();
                    }
                }

                if (parameters.ContainsKey("RepoName"))
                {
                    Title = parameters.GetValue<string>("RepoName");
                }


                    base.OnNavigatedTo(parameters);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }

        private void GetItems()
        {
            foreach (var item in MasterPullRequests.Skip(Skip).Take(PageSize * Increment).ToList())
            {

                PullRequests.Add(item);
            }
            Skip += PageSize;
            PageSize++;

            OpenLabel = MasterPullRequests.Count(c=>c.State==ItemState.Open) + " open";
            ClosedLabel =" / " + MasterPullRequests.Count(c => c.State == ItemState.Open) + " closed"; 
        }

        void GoToItem(object obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("PRUrl", obj.ToString());
            _navigationService.NavigateAsync($"{nameof(WebBrowserPage)}", parameters);
        }
    }
}
