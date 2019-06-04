using System.Collections.Generic;
using System.Threading.Tasks;
using GithubRepoExample.Models;
using Octokit;

namespace GithubRepoExample.Interfaces
{
    public interface IGithubService
    {
        Task<List<Models.Repository>> GetRepositories(int paging);
        Task<IReadOnlyList<PullRequest>> GetRepository(long url);
    }
}
