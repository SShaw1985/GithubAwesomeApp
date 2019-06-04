using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using GithubRepoExample.Interfaces;
using GithubRepoExample.Models;
using GithubRepoExample.Services;
using GithubRepoExample.Views;
using Octokit;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace GithubRepoExample.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private IGithubService Github { get; set; }

        public ObservableCollection<Models.Repository> PRItems { get; set; }

        public ObservableCollection<Models.Repository> MasterPRItems { get; set; }

        public string SearchTerm { get; set; }

        public bool ShowLoadMore { get; set; }

        public int Paging { get; set; } = 1;

        public ICommand GoToItemCommand { get { return new Command(GoToItem); }}

        public ICommand SearchCommand { get { return new Command(Search); } }

        public ICommand LoadMoreCommand { get { return new Command(LoadMore); } }

        public string ErrorMessage { get; set; }

        public HomePageViewModel(IAppNavigationService navigationService, IPageDialogService pageDialogService,
                                 IDeviceService deviceService, IGithubService gitService)
            : base(navigationService, pageDialogService, deviceService)
        {
            Github = gitService;
            Title = "Github Awesome";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            base.OnNavigatedTo(parameters);

            GetData();
        }

        private async void GetData()
        {
            try
            {
                if(MasterPRItems!=null && MasterPRItems.Count>0)
                {
                    return;
                }

                await Task.Delay(1000);
                UserDialogs.Instance.ShowLoading();
                MasterPRItems = new ObservableCollection<Models.Repository>();
                PRItems = new ObservableCollection<Models.Repository>();
                var model = await Github.GetRepositories(Paging);
                Paging = model.Count / 99;
                MasterPRItems = new ObservableCollection<Models.Repository>(model);
                PRItems = MasterPRItems;
                ErrorMessage = string.Empty;
                if (MasterPRItems.Count >= 999)
                {
                    ShowLoadMore = false;
                }
                else
                { 
                    ShowLoadMore = true; 
                }

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        private async void GoToItem(object obj)
        {
            if(!string.IsNullOrEmpty(obj.ToString()))
            {
                var parameters = new NavigationParameters();
                parameters.Add("PRID", obj.ToString());
                if (PRItems != null)
                {
                    var name = PRItems.Where(x => x.Id.ToString() == obj.ToString()).Select(x => x.Name).FirstOrDefault();
                    parameters.Add("RepoName", name);
                }

                if (_navigationService != null)
                {
                    try
                    {
                        await _navigationService.NavigateAsync($"{nameof(ItemDetailsPage)}", parameters);
                    }
                    catch (Exception ex)
                    {
                        string sss = ex.Message;
                    }
                }
            }
            else
            {
                ErrorMessage = "An error occurred";
            }
        }

        private void Search()
        {
            if (SearchTerm != null)
            {
                PRItems = new ObservableCollection<Models.Repository>(MasterPRItems.Where(c => c.Name.ToLower().Contains(SearchTerm.ToLower()) || (c.Owner!=null && c.Owner.Login!=null && c.Owner.Login.ToLower().Contains(SearchTerm.ToLower()))).ToList());
            }
            else
            {
                PRItems = MasterPRItems;
            }
        }

        private async void LoadMore(object obj)
        {
            if (Paging < 11)
            {
                UserDialogs.Instance.ShowLoading();

                Paging++;

                var model = await Github.GetRepositories(Paging);
                MasterPRItems = new ObservableCollection<Models.Repository>(model);
               
                Search();

                if (MasterPRItems.Count>=999)
                {
                    ShowLoadMore = false;
                }
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
