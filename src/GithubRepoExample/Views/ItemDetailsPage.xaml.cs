using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GithubRepoExample.Views
{
    public partial class ItemDetailsPage : ContentPage
    {
        public ItemDetailsPage()
        {
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var lst = sender as ListView;

            lst.SelectedItem = null;
        }
    }
}
