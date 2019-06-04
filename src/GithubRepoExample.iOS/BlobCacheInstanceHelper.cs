using System;
using System.IO;
using System.Reactive.Linq;
using Akavache;
using Akavache.Sqlite3;
using GithubRepoExample.Interfaces;
using Splat;

namespace GithubRepoExample.iOS
{
    public class BlobCacheInstanceHelper : IBlobCacheInstanceHelper
    {
        private IFilesystemProvider _filesystemProvider;
        public BlobCacheInstanceHelper() { }
        public void Init()
        {
            _filesystemProvider = Locator.Current.GetService<IFilesystemProvider>();
            GetLocalMachineCache();
        }
        public IBlobCache LocalMachineCache { get; set; }
        private void GetLocalMachineCache()
        {
            var localCache = new Lazy<IBlobCache>(() =>
            {
                _filesystemProvider.CreateRecursive(_filesystemProvider.GetDefaultLocalMachineCacheDirectory()).SubscribeOn(BlobCache.TaskpoolScheduler).Wait();
                return new SQLitePersistentBlobCache(Path.Combine(_filesystemProvider.GetDefaultLocalMachineCacheDirectory(), "blobs.db"), BlobCache.TaskpoolScheduler);
            });
            this.LocalMachineCache = localCache.Value;
        }
    }
}

