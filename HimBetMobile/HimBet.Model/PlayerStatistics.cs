using System;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
namespace HemBit.Model
{
    public class PlayerStatistics : IPersistant
    {
        [JsonIgnore]
        [BsonIdAttribute]
        public string Id { get { return $"P{PlayerId}D{Date}"; } set { throw new FieldAccessException(); } }
        [JsonIgnore]

        public string PlayerId { get; set; }
        [JsonIgnore]
        public string GameId { get; set; }
        [BsonElement("Rk")]
        [JsonProperty("Rk")]
        public int TeamGameId { get; set; }
        [BsonElement("G")]
        [JsonProperty("G")]
        public int PlayerGameId { get; set; }
        public DateTime Date { get; set; }
        [BsonElement("Tm")]
        [JsonProperty("Tm")]
        public string PlayerTeamCode { get; set; }
        [BsonElement("Opp")]
        [JsonProperty("Opp")]
        public string OppositeTeamCode { get; set; }
        [BsonElement("GS")]
        [JsonProperty("GS")]
        public bool GameStarter { get; set; }
        public short PlayerTeamScore { get; set; }
        [BsonElement("MP")]
        [JsonProperty("MP")]
        public TimeSpan? MinutesPlayed { get; set; }
        [BsonElement("FG")]
        [JsonProperty("FG")]
        public short FieldGoals { get; set; }
        [BsonElement("FGA")]
        [JsonProperty("FGA")]
        public short FieldGoalAttempts { get; set; }
        [BsonIgnore]
        [JsonProperty("FGP")]
        public decimal FieldGoalsPercentage { get => FieldGoalAttempts != 0 ? FieldGoals / FieldGoalAttempts:0; private set { } }
        [BsonElement("3P")]
        [JsonProperty("3P")]
        public short ThreePointsGoals { get; set; }
        [BsonElement("3PA")]
        [JsonProperty("3PA")]
        public short ThreePointsGoalAttempts { get; set; }
        [BsonIgnore]
        [JsonProperty("3PP")]
        public decimal ThreePointsGoalsPercentage { get => ThreePointsGoalAttempts!=0?ThreePointsGoals / ThreePointsGoalAttempts:0; private set { } }
        [BsonElement("FT")]
        [JsonProperty("FT")]
        public short FreeThrows { get; set; }
        [BsonElement("FTA")]
        [JsonProperty("FTA")]
        public short FreeThrowsAttempts { get; set; }
        [BsonIgnore]
        [JsonProperty("FTP")]
        public decimal FreeThrowsPercentage { get => FreeThrowsAttempts!=0? FreeThrows / FreeThrowsAttempts:0; private set { } }
        [BsonElement("ORB")]
        [JsonProperty("ORB")]
        public short OffensiveRebounbds { get; set; }
        [BsonElement("DRB")]
        [JsonProperty("DRB")]
        public short DeffensiveRebounds { get; set; }
        [BsonIgnore]
        [JsonProperty("TRB")]
        public int TotalRebounds { get => OffensiveRebounbds + DeffensiveRebounds; private set { } }
        [BsonElement("AST")]
        [JsonProperty("AST")]
        public short Assists { get; set; }
        [BsonElement("STL")]
        [JsonProperty("STL")]
        public short Steals { get; set; }
        [BsonElement("BLK")]
        [JsonProperty("BLK")]
        public short Blocks { get; set; }
        [BsonElement("TOV")]
        [JsonProperty("TOV")]
        public short Turnovers { get; set; }
        [BsonElement("PF")]
        [JsonProperty("PF")]
        public short PersonaFaults { get; set; }
        [BsonElement("PTS")]
        [JsonProperty("PTS")]
        public short PointsScored { get; set; }
        [BsonIgnore]
        [JsonProperty("GmSc")]
        public double GameScore { get => CalculateGameScore(); private set { } }
        [BsonElement("PlusMinus")]
        [JsonProperty("PlusMinus")]
        public short OverallImpactOnTeamSuccess { get; set; }
        public string Age { get; set; }

        private double CalculateGameScore()
        {
            
            return  PointsScored +
                    4 * FieldGoals +
                    .7 * OffensiveRebounbds +
                    .3 * DeffensiveRebounds +
                    Steals +
                    .7 * Assists +
                    .7 * Blocks -
                    .7 * FieldGoalAttempts -
                    .4 * FreeThrows -
                    .4 * PersonaFaults -
                    Turnovers;
        }

        public PlayerStatistics()
        {
        }
    }
}
