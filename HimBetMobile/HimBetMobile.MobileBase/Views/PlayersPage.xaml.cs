using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using HimBetMobile.Models;
using HimBetMobile.Views;
using HimBetMobile.ViewModels;
using HemBit.Model;

namespace HimBetMobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class PlayersPage : ContentPage
    {
        PlayersViewModel viewModel;

        public PlayersPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new PlayersViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
             var player = args.SelectedItem as Player;
            if (player == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new PlayerDetailViewModel(player)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;  
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Players.Count == 0)
                viewModel.LoadPlayersCommand.Execute(null);
        }
    }
}