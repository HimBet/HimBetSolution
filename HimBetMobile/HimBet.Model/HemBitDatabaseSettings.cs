using System;
namespace HemBit.Model
{
    public class HemBitDatabaseSettings : IHemBitDatabaseSettings
    {
        public string TeamsCollectionName { get; set; }
        public string PlayersCollectionName { get; set; }
        public string GamesCollectionName { get; set; }
        public string PlayersScoresCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public HemBitDatabaseSettings()
        {

        }
    }
}
