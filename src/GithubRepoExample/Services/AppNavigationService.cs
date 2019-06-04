using System;
using System.Threading.Tasks;
using GithubRepoExample.Interfaces;
using Prism.Navigation;

namespace GithubRepoExample.Services
{
    public class AppNavigationService : IAppNavigationService
    {
        private INavigationService Navigation { get; set; }

        public AppNavigationService(INavigationService navi)
        {
            Navigation = navi;
        }

        public Task PopToRootAsync(NavigationParameters parameters)
        {
            return Navigation.GoBackToRootAsync(parameters);
        }


        public Task<INavigationResult> GoBackAsync()
        {
            return Navigation.GoBackAsync();
        }

        public Task<INavigationResult> GoBackAsync(INavigationParameters parameters)
        {
            return Navigation.GoBackAsync(parameters);
        }

        public Task<INavigationResult> NavigateAsync(Uri uri)
        {
            return Navigation.NavigateAsync(uri);
        }

        public Task<INavigationResult> NavigateAsync(Uri uri, INavigationParameters parameters)
        {
            return Navigation.NavigateAsync(uri,parameters);
        }

        public Task<INavigationResult> NavigateAsync(string name)
        {
            return Navigation.NavigateAsync(name);
        }

        public Task<INavigationResult> NavigateAsync(string name, INavigationParameters parameters)
        {
            return Navigation.NavigateAsync(name,parameters);
        }
    }
}
