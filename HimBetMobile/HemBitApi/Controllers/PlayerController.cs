    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemBit.Model;
using HemBit.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HemBitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase, IPlayerController
    {
        private readonly PlayerDBService _playerDBService;
        public PlayerController(PlayerDBService playerDBService)
        {
            _playerDBService = playerDBService;     
        }

        // GET api/player/Michael/Jordan
        [HttpGet("{firstName}/{lastName}" )]
        public ActionResult<List<Player>> Get(string firstName, string lastName)
        {
            var players= _playerDBService.Get(firstName, lastName);
            if (players == null || players.Count==0)
            {
                return NotFound();
            }

            return players;
        }
        // GET api/player/1
        [HttpGet("{id}", Name = "GetPlayer")]
        public ActionResult<Player> Get(string id)
        {
            var player= _playerDBService.Get(id);
            if (player == null  )
            {
                return NotFound();
            }
            return player;
        }
        [HttpGet]
        public ActionResult<List<Player>> Get()
        {
            var players = _playerDBService.Get();
            if (players == null || players.Count == 0)
            {
                return NotFound();
            }

            return players;
        }

        [HttpPost]
        public ActionResult<Player> Post(Player player)
        {
            _playerDBService.Create(player);
            return CreatedAtRoute("GetPlayer", new { id = player.Id.ToString() }, player);
        }


    }
}
