using System;
using System.Linq;
using HemBit.Model;
using HimBetMobile.Models;

namespace HimBetMobile.ViewModels
{
    public class PlayerDetailViewModel : BaseViewModel
    {
        public Player Player { get; set; }
        public DateTime? FirstKnownGame { get; set; }
        public DateTime? LastKnownGame { get; set; }
        public double FirstYear { get; }
        public double LastYear { get; }
    

        public PlayerDetailViewModel(Player player = null)
        {
            Title =$"{player?.FirstName} {player?.LastName}";
            Player = player;
            FirstKnownGame = player.Teams?.Select(t=>t.StartDate).Min();
            LastKnownGame = player.Teams?.Select(t => t.LastKnownPlayDate??DateTime.Now).Max();
            FirstYear = FirstKnownGame.GetValueOrDefault().Year;
            LastYear = LastKnownGame.GetValueOrDefault().Year;
          
        }
    }
}
