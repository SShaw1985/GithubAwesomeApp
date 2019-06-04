using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GithubRepoExample.Interfaces;
using Plugin.Connectivity;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace GithubRepoExample.ViewModels
{
	public class WebBrowserPageViewModel : ViewModelBase
    {
        public string WebSource { get; set; }
        public double DeviceHeight { get; set; }
        public double DeviceWidth { get; set; }

        public WebBrowserPageViewModel(IAppNavigationService navigationService, IPageDialogService pageDialogService,
                                 IDeviceService deviceService)
            : base(navigationService, pageDialogService, deviceService)
        {
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                UserDialogs.Instance.ShowLoading(); 
                if (CrossConnectivity.Current.IsConnected)
                {
                    DeviceHeight = Application.Current.MainPage.Height;
                    DeviceWidth = Application.Current.MainPage.Width;
                    WebSource = parameters.GetValue<string>("PRUrl");
                    // Delay to keep the spinner up with the page loads
                    await Task.Delay(2000);
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("No internet connection");
                    await _navigationService.GoBackAsync();
                }

            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }

        }
    }
}
