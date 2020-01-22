using System;
using System.Collections.Generic;
using HemBit.Model;
using Microsoft.AspNetCore.Mvc;

namespace HemBitApi.Controllers
{
    public interface IPlayerStatisticsController
    {
        ActionResult<List<PlayerStatistics>> GetAllPlayerStatistics(string playerId);
        ActionResult<List<PlayerStatistics>> GetPlayerStatisticsForPeriod(string playerId, DateTime startDate, DateTime endDate);
    }
}