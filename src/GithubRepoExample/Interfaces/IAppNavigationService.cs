using System;
using System.Threading.Tasks;
using Prism.Navigation;

namespace GithubRepoExample.Interfaces
{
    public interface IAppNavigationService : INavigationService
    {
        Task PopToRootAsync(NavigationParameters parameters);
    }

}
