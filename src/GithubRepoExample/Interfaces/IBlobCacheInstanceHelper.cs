using System;
using Akavache;

namespace GithubRepoExample.Interfaces
{
    public interface IBlobCacheInstanceHelper
    {
        void Init();
        IBlobCache LocalMachineCache { get; set; }
    }
}
