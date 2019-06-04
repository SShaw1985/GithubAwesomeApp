
using System;
using System.Collections.Generic;
using GithubRepoExample.Models;
using GithubRepoExample.ViewModels;
using Prism.Events;
using Xamarin.Forms;

namespace GithubRepoExample.Views
{
    public partial class HomePage : ContentPage
    {
        HomePageViewModel HomeViewModel { get; set; }

        public HomePage()
        {
            try
            {
                InitializeComponent();
            }
            catch(Exception e)
            {
                string ss = e.Message;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(HomeViewModel==null)
            {
                HomeViewModel = BindingContext as HomePageViewModel;
            }
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var lst = sender as ListView;
          
            lst.SelectedItem = null;
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            HomeViewModel.SearchCommand.Execute(null);
        }
    }
}
