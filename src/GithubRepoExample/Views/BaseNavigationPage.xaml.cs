using System;
using System.Collections.Generic;
using Prism.Navigation;
using Xamarin.Forms;

namespace GithubRepoExample.Views
{

    public partial class BaseNavigationPage : NavigationPage, INavigationPageOptions, IDestructible
    {
        public BaseNavigationPage()
        {
            InitializeComponent();
        }

        public bool ClearNavigationStackOnNavigation
        {
            get { return true; }
        }

        public void Destroy()
        {

        }
    }
}
