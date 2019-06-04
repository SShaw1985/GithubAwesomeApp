using System;
using System.Collections.Generic;
using System.Linq;
using GithubRepoExample.Interfaces;
using Prism;
using Prism.Ioc;

namespace GithubRepoExample.iOS
{
    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.Register<IBlobCacheInstanceHelper, BlobCacheInstanceHelper>();
        }
    }
}
