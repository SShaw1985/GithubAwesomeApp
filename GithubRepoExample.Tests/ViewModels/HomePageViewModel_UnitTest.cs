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
    public class HomePageViewModel_UnitTest
    {
        readonly MockRepository mockRepository;

        private HomePageViewModel viewModelUnderTest;

        public Mock<IAppNavigationService> Navigation { get; set; }

        public Mock<IPageDialogService> PageDialog { get; set; }

        public Mock<IDeviceService> DeviceSettings { get; set; }

        public Mock<IGithubService> GithubService { get; set; }

        public HomePageViewModel_UnitTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose);
        }

        [Fact]
        public void TestOnNavigated()
        {
            SetUpDefault();

            GithubService.Setup(c => c.GetRepositories()).Returns(() => { return Task.Run(() => { return new Models.PullRequests(){ items = new System.Collections.Generic.List<Models.PRStub>()}; }); });

            SetUpModel();

            var param = new NavigationParameters();

            viewModelUnderTest.OnNavigatedTo(param);

            Assert.True(viewModelUnderTest.PRItems != null);
        }


        [Fact]
        public void TestPageHasTitle()
        {
            SetUpDefault();

            GithubService.Setup(c => c.GetRepositories()).Returns(() => { return Task.Run(() => { return new Models.PullRequests(); }); });

            SetUpModel();

            var param = new NavigationParameters();

            viewModelUnderTest.OnNavigatedTo(param);

            Assert.True(!string.IsNullOrEmpty(viewModelUnderTest.Title) && viewModelUnderTest.Title== "Home Page");
        }

        [Fact]
        public void TestNavigatedTo()
        {
            SetUpDefault();

            Navigation.Setup(c => c.NavigateAsync(It.IsAny<string>(), It.IsAny<NavigationParameters>())).Verifiable();
            Navigation.Setup(c => c.NavigateAsync(It.IsAny<string>())).Verifiable();
            Navigation.Setup(c => c.NavigateAsync(It.IsAny<Uri>())).Verifiable();
            Navigation.Setup(c => c.NavigateAsync(It.IsAny<Uri>(), It.IsAny<NavigationParameters>())).Verifiable();

            SetUpModel();

            viewModelUnderTest.GoToItemCommand.Execute(11000);

            Assert.True(string.IsNullOrEmpty(viewModelUnderTest.ErrorMessage));
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
            viewModelUnderTest = new HomePageViewModel(Navigation.Object, PageDialog.Object, DeviceSettings.Object, GithubService.Object);
        }
    }
}
