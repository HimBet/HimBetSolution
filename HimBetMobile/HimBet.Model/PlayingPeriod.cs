using System;

namespace HemBit.Model
{
    public class PlayingPeriod : IPersistant
    {
        public Team Team { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? LastKnownPlayDate { get; set; }
        public string Id { get ; set; }
    }
}