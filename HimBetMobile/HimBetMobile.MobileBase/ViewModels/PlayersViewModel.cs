using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using HimBetMobile.Models;
using HimBetMobile.Views;
using HemBit.Model;

namespace HimBetMobile.ViewModels
{
    public class PlayersViewModel : BaseViewModel
    {
        public ObservableCollection<Player> Players { get; set; }
        public Command LoadPlayersCommand { get; set; }

        public PlayersViewModel()
        {
            Title = "Browse";
            Players = new ObservableCollection<Player>();
            LoadPlayersCommand = new Command(async () => await ExecuteLoadPlayersCommand());

            /*MessagingCenter.Subscribe<NewItemPage, Player>(this, "AddItem", async (obj, player) =>
            {
                var newPlayer = player as Player;
                Players.Add(newPlayer);
                await PlayerDataStore.AddItemAsync(newPlayer);
            });*/
        }

        async Task ExecuteLoadPlayersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Players.Clear();
                var players = await PlayerDataStore.GetItemsAsync(true);
                foreach (var player in players)
                {
                    Players.Add(player);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}