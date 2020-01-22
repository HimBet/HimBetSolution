using System;
using System.Collections.Generic;
using System.Reflection;
using HemBit.Model;
using HemBit.Services;
using Microsoft.AspNetCore.Mvc;

namespace HemBitApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlayerStatisticsController : ControllerBase, IPlayerStatisticsController
    {
        private readonly PlayerStatisticsDBService _playerStatisticsDBService;
        public PlayerStatisticsController(PlayerStatisticsDBService playerStatisticsDBService)
        {
            _playerStatisticsDBService = playerStatisticsDBService;
        }


        [HttpGet("{playerId}")]
        public ActionResult<List<PlayerStatistics>> GetAllPlayerStatistics(string playerId)
        {
            var retval = _playerStatisticsDBService.GetAllPlayerStatistics(playerId);
            if (retval == null)
            {
                return NotFound();
            }
            return retval;
        }


        [HttpGet("{playerId}/{startDate}/{endDate}")]
        public ActionResult<List<PlayerStatistics>> GetPlayerStatisticsForPeriod(string playerId, DateTime startDate, DateTime endDate)
        {
            var retval = _playerStatisticsDBService.GetPlayerStatisticsForPeriod(playerId, startDate, endDate);
            if (retval == null)
            {
                return NotFound();
            }
            return retval;
        }

        [HttpPost("{playerId}")]
        public void BulkInsertPlayerStatistics(IEnumerable<PlayerStatistics> playerStatistics)
        {
            
            _playerStatisticsDBService.BulkInsert(playerStatistics);
        }
    }


}
