using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;
using HemBit.Model;
using HemBit.Services;

namespace HemBit.Server.PlayerStatisticLoader
{
    class Program
    {
        private static readonly HemBitDatabaseSettings _dbSettings = new HemBitDatabaseSettings {
            ConnectionString = "mongodb://localhost",
            DatabaseName = "HemBit"
        };
        private static readonly PlayerDBService _playerDBService = new PlayerDBService(_dbSettings);
        private static readonly PlayerStatisticsDBService _playerStatisticsDBService = new PlayerStatisticsDBService(_dbSettings);
        static void Main(string[] args)
        {
            string filesPath;
            if (args.Length > 0)
                filesPath = args[0];
            else
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                filesPath = Path.Combine(currentDirectory, "Import");
                

            }
            var playersFolders = Directory.GetDirectories(filesPath);
            foreach (var playerFolder in playersFolders)
            {
                ParseSinglePlayerFiles(playerFolder);
            }
        }

        private static void ParseSinglePlayerFiles(string playerFolder)
        {
            var files = Directory.GetFiles(playerFolder, "*.csv", SearchOption.AllDirectories);
            if (files.Length == 0) return;
            var directoryInfo = new DirectoryInfo(playerFolder);
            var player = GetPlayer(directoryInfo, SportType.BasketBall);

            List<PlayingPeriod> playerTeams = GetPlayerTeams(player);
            foreach (var fileName in files)
            {
                ParseFile(fileName,player,playerTeams);
            }
            UpdatePlayer(player,playerTeams);

        }

        private static void UpdatePlayer(Player player, List<PlayingPeriod> playerTeams)
        {
            player.Teams = playerTeams;
            _playerDBService.Update(player.Id, player);
        }

        private static List<PlayingPeriod> GetPlayerTeams(Player player)
        {
            return player.Teams??new List<PlayingPeriod>();
        }

        private static void ParseFile(string fileName, Player player, List<PlayingPeriod> playerTeams)
        {
            bool hasErrors = false;
            using (var file = new StreamReader(fileName))
            {
                using (var errorFile = new StreamWriter($"{fileName}.errlog"))
                {
                    List<PlayerStatistics> playerStatistics = new List<PlayerStatistics>();
                    var header = file.ReadLine();
                    string data=String.Empty;
                    int i = 0;
                    
                    try
                    {

                        while ((data = file.ReadLine()) != null)
                        {
                            var dataArray = data.Split(',');
                            bool inactive = (dataArray[8] != "1");
                            var playerStat = new PlayerStatistics
                            {
                                PlayerId = player.Id,
                                TeamGameId = GetInt(dataArray, 0, false),
                                PlayerGameId = GetInt(dataArray, 1, inactive),
                                Date = DateTime.ParseExact(dataArray[2], "dd/MM/yyyy", null),
                                Age = dataArray[3],
                                PlayerTeamCode = dataArray[4],
                                OppositeTeamCode = dataArray[6],
                                GameStarter = inactive || dataArray[8] == "0" ? false : true,
                                MinutesPlayed = GetMinutesPlayed(dataArray, inactive),
                                FieldGoalAttempts = GetShort(dataArray, 11, inactive),
                                ThreePointsGoals = GetShort(dataArray, 13, inactive),
                                ThreePointsGoalAttempts = GetShort(dataArray, 14, inactive),
                                FreeThrows = GetShort(dataArray, 16, inactive),
                                FreeThrowsAttempts = GetShort(dataArray, 17, inactive),
                                OffensiveRebounbds = GetShort(dataArray, 19, inactive),
                                DeffensiveRebounds = GetShort(dataArray, 20, inactive),
                                Assists = GetShort(dataArray, 22, inactive),
                                Steals = GetShort(dataArray, 23, inactive),
                                Blocks = GetShort(dataArray, 24, inactive),
                                Turnovers = GetShort(dataArray, 25, inactive),
                                PersonaFaults = GetShort(dataArray, 26, inactive),
                                PointsScored = GetShort(dataArray, 27, inactive),
                                OverallImpactOnTeamSuccess = dataArray.Length<30 ||String.IsNullOrEmpty(dataArray[29])?(short)0:GetShort(dataArray, 29, inactive)

                            };
                            SetPlayerTeam(playerTeams, playerStat);
                            playerStatistics.Add
                            (
                                playerStat
                            );
                            i++;
                        }
                        UpdatePlayerStatistics(player, playerStatistics);
                    }
                    catch (Exception e)
                    {
                        hasErrors = true;
                        if (e.InnerException!=null)
                            errorFile.WriteLine($"{data},,{e.Message},{e.InnerException.Message}");
                        else
                            errorFile.WriteLine($"{data},,{e.Message}");
                        Console.WriteLine(e);
                    }
                }
             }
            try
            {
                if (hasErrors)
                    File.Move(fileName, $"{fileName}.error");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to move file {fileName}");
            }
        }

        private static void UpdatePlayerStatistics(Player player, List<PlayerStatistics> playerStatistics)
        {
            _playerStatisticsDBService.BulkInsert(playerStatistics);
        }

        private static void SetPlayerTeam(List<PlayingPeriod> playerTeams, PlayerStatistics playerStat)
        {
            var foundPeriod = playerTeams.Find(pp => pp.Team.Code == playerStat.PlayerTeamCode);

            if (foundPeriod == null)
            {
                foundPeriod = new PlayingPeriod
                {
                    Id = Guid.NewGuid().ToString(),
                    Team = new Team { Code = playerStat.PlayerTeamCode, SportType = SportType.BasketBall },
                    StartDate = playerStat.Date,
                    LastKnownPlayDate = playerStat.Date
                };
                playerTeams.Add(foundPeriod);
            }
            
            if (foundPeriod.StartDate >= playerStat.Date)
            {
                if (foundPeriod.LastKnownPlayDate == null || foundPeriod.LastKnownPlayDate >= playerStat.Date)
                    foundPeriod.StartDate = playerStat.Date;
            }
            else
            {
                if (foundPeriod.LastKnownPlayDate == null || foundPeriod.LastKnownPlayDate <= playerStat.Date)
                    foundPeriod.LastKnownPlayDate = playerStat.Date;
            }
            

        }

        private static Player GetPlayer(DirectoryInfo directory, SportType sportType)
        {
            var fullPlayerName = directory.Name;
            var split = fullPlayerName.Split(" ");
            List<Player> players;
            Player player;
            string firstName = split[0];
            string lastName = split.Length > 1 ? split[1] : "";
            players = _playerDBService.Get(firstName, lastName);
            if (players == null || players.Count==0)
                players= _playerDBService.Get(lastName, firstName);
            if (players != null && players.Count > 0)
                player= players[0];
            else
                player= _playerDBService.Create(new Player
                {
                    Id=Guid.NewGuid().ToString(),
                    FirstName=firstName,
                    LastName=lastName,
                    SportType=sportType
                    
                });
            
            return player;
        }

        

        private static TimeSpan? GetMinutesPlayed(string[] dataArray, bool inactive)
        {

            TimeSpan? time = null;
            if (!inactive)
            {
                if (dataArray[9].Length == 8)
                    time = TimeSpan.ParseExact(dataArray[9], @"m\:ss\:ff", null);
                else
                    time = TimeSpan.ParseExact(dataArray[9], @"m\:ss", null);
            }

            return time;
        }

        private static int GetInt(string[] dataArray, int collumnNumber, bool returnDefaultValue)
        {
            if (dataArray.Length < collumnNumber - 1) return  0;
            try
            {
                return returnDefaultValue ?  0 : int.Parse(dataArray[collumnNumber]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to parse data in collumn {collumnNumber} to int", ex);
            }
        }
        private static short GetShort(string[] dataArray, int collumnNumber, bool returnDefaultValue)
        {
            if (dataArray.Length < collumnNumber-1) return (short)0;
            try
            {
                return returnDefaultValue ? (short)0 : short.Parse(dataArray[collumnNumber]);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to parse data in collumn {collumnNumber} to short",ex);
            }
        }
    }
}
