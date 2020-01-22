namespace HemBit.Model
{
    public interface IHemBitDatabaseSettings
    {
        string TeamsCollectionName { get; set; }
        string PlayersCollectionName { get; set; }
        string GamesCollectionName { get; set; }
        string PlayersScoresCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}