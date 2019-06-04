using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Akavache;
using GithubRepoExample.Interfaces;
using GithubRepoExample.Models;
using MonkeyCache;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Octokit;
using BarrelSQL = MonkeyCache.FileStore.Barrel;

namespace GithubRepoExample.Services
{
    public class GithubService : IGithubService
    {
        private IBarrel Sql;
        private JsonSerializerSettings SerializerSettings { get; set; }

        public GithubService()
        {

            BarrelSQL.ApplicationId = "com.refractored.monkeycachetestlite";
            Sql = BarrelSQL.Current;
            SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
        }

        public async Task<List<Models.Repository>> GetRepositories(int paging)
        {
            // await BlobCache.UserAccount.Invalidate("Repositories");

            var foundInCache = true;
            try
            {
                var check = await BlobCache.UserAccount.GetObject<string>("Repositories");
                if(string.IsNullOrEmpty(check))
                {
                    foundInCache = false;
                }
            }
            catch(Exception ex)
            {
                foundInCache = false;
            }

            if (!foundInCache )
            {
                var github = new GitHubClient(new ProductHeaderValue("kolpime"));
                github.Credentials = new Credentials("18d33d49716bf4847b12ed96a42ef5da66ede197");
                var repo = await github.Search.SearchRepo(new SearchRepositoriesRequest() { Language = Language.JavaScript, SortField = RepoSearchSort.Stars, Archived=false, Page = paging, PerPage = 99});
                await BlobCache.UserAccount.InsertObject<string>("Repositories", JsonConvert.SerializeObject(repo.Items.ToList()), new DateTimeOffset(DateTime.Now.AddDays(1)));
                var r1 = await BlobCache.UserAccount.GetObject<string>("Repositories");
            }

            var json = await BlobCache.UserAccount.GetObject<string>("Repositories");
            var retVal = JsonConvert.DeserializeObject<List<Models.Repository>>(json);

            // get a new page and add it
            if(retVal.Count/99!=paging && retVal.Count() <= 999)
            {
                var github = new GitHubClient(new ProductHeaderValue("kolpime"));
                github.Credentials = new Credentials("18d33d49716bf4847b12ed96a42ef5da66ede197");
                var repo = await github.Search.SearchRepo(new SearchRepositoriesRequest() { Language = Language.JavaScript, SortField = RepoSearchSort.Stars, Archived = false, Page = paging, PerPage = 99 });

               
                var oldjson = await BlobCache.UserAccount.GetObject<string>("Repositories");
                var newjson = JsonConvert.SerializeObject(repo.Items.ToList());
                var oldvalues = JsonConvert.DeserializeObject<List<Models.Repository>>(oldjson);
                var newvalues = JsonConvert.DeserializeObject<List<Models.Repository>>(newjson);

                foreach(var item in newvalues)
                {
                    oldvalues.Add(item);
                }

                // invalidate and reset for new page
                await BlobCache.UserAccount.Invalidate("Repositories");
                await BlobCache.UserAccount.InsertObject<string>("Repositories", JsonConvert.SerializeObject(oldvalues), new DateTimeOffset(DateTime.Now.AddDays(1)));


                json = await BlobCache.UserAccount.GetObject<string>("Repositories");
                retVal = JsonConvert.DeserializeObject<List<Models.Repository>>(json);
            }

           

            return retVal;
        }

        public async Task<IReadOnlyList<PullRequest>> GetRepository(long id)
        {
            var github = new GitHubClient(new ProductHeaderValue("kolpime"));
            github.Credentials = new Credentials("18d33d49716bf4847b12ed96a42ef5da66ede197");
            return await github.PullRequest.GetAllForRepository(id);
        }
    }
}
