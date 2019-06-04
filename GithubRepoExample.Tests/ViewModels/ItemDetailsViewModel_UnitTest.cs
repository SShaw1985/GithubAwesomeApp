using System;
using System.Threading.Tasks;
using GithubRepoExample.Interfaces;
using GithubRepoExample.ViewModels;
using Moq;
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

        public ItemDetailsViewModel_UnitTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
        }

        [Fact]
        public void TestValidID()
        {
            SetUpDefault();

            GithubService.Setup(c => c.GetRepository(It.IsAny<int>())).Returns(() => { return null; }).Verifiable();

            SetUpModel();

            var param = new NavigationParameters();
            param.Add("PRID", 11000);
            viewModelUnderTest.OnNavigatedTo(param);

            Assert.True(viewModelUnderTest.PullRequest==null);
        }

        [Fact]
        public void TestInValidID()
        {
            SetUpDefault();

            GithubService.Setup(c => c.GetRepository(It.IsAny<int>())).Returns(() => { return Task.Run(() => { return new Models.PRStub(); }); });

            SetUpModel();

            var param = new NavigationParameters();
            param.Add("PRID", 11000);
            viewModelUnderTest.OnNavigatedTo(param);

            Assert.True(viewModelUnderTest.PullRequest != null);
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
}
