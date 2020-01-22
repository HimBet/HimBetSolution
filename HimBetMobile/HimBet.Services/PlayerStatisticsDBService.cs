using System;
using System.Collections.Generic;
using HemBit.Model;
 
using MongoDB.Driver;

namespace HemBit.Services
{
    public class PlayerStatisticsDBService : HemBitDBService<PlayerStatistics>
    {
        public PlayerStatisticsDBService(IHemBitDatabaseSettings settings) : base(settings)
        {

        }
        public List<PlayerStatistics> GetAllPlayerStatistics(string playerId) =>
           _items.Find<PlayerStatistics>(item => ((PlayerStatistics)item).PlayerId == playerId

           ).ToList();
        public List<PlayerStatistics> GetPlayerStatisticsForPeriod(string playerId, DateTime periodStartDate, DateTime periodEndDate) =>
            _items.Find<PlayerStatistics>(item => ((PlayerStatistics)item).PlayerId == playerId
                                     && ((PlayerStatistics)item).Date >= periodStartDate
                                     && ((PlayerStatistics)item).Date <= periodEndDate
            ).ToList();
    }
}
