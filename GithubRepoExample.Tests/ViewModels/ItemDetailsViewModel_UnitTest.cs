using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using GithubRepoExample.Interfaces;
using GithubRepoExample.ViewModels;
using Moq;
using Octokit;
using Prism.Navigation;
using Prism.Services;
using Xunit;

namespace GithubRepoExample.Tests.ViewModels
{
    public class ItemDetailsViewModel_UnitTest
    {

        readonly MockRepository mockRepository;

        private ItemDetailsViewModel viewModelUnderTest;

        public Mock<IAppNavigationService> Navigation { get; set; }

        public Mock<IPageDialogService> PageDialog { get; set; }

        public Mock<IDeviceService> DeviceSettings { get; set; }

        public Mock<IGithubService> GithubService { get; set; }

        public IReadOnlyList<PullRequest> Values { get; set; }

        public ItemDetailsViewModel_UnitTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
        }

        [Fact]
        public void TestInValidID()
        {
            SetUpDefault();

            GithubService.Setup(c => c.GetRepository(It.IsAny<int>())).Returns(Task.Run(()=>
            {
                Values = new List<PullRequest>();
                return Values;
            }));
            var userDialogsMock = mockRepository.Create<IUserDialogs>();
            UserDialogs.Instance = userDialogsMock.Object;
            SetUpModel();

            var param = new NavigationParameters();
            param.Add("PRID", 11000);
            viewModelUnderTest.OnNavigatedTo(param);

            Assert.True(viewModelUnderTest.MasterPullRequests==null);
        }

        private IReadOnlyList<PullRequest> GenerateList()
        {
            var retVal = new List<PullRequest>();
            retVal.Add(new PullRequest());
            return retVal;
        }

        void InitialiseMocks()
        {
            Navigation = mockRepository.Create<IAppNavigationService>();
            PageDialog = mockRepository.Create<IPageDialogService>();
            DeviceSettings = mockRepository.Create<IDeviceService>();
            GithubService = mockRepository.Create<IGithubService>();
        }

        private void SetUpDefault()
        {
            InitialiseMocks();
        }

        private void SetUpModel()
        {
            viewModelUnderTest = new ItemDetailsViewModel(Navigation.Object, PageDialog.Object, DeviceSettings.Object, GithubService.Object);
        }
    }

    public class ReadOnlyList<PullRequest> : IReadOnlyList<PullRequest>
    {
        public PullRequest this[int index] => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public IEnumerator<PullRequest> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
