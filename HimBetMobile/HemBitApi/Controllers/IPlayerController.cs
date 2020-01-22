using System.Collections.Generic;
using HemBit.Model;
using Microsoft.AspNetCore.Mvc;

namespace HemBitApi.Controllers
{
    public interface IPlayerController
    {
        ActionResult<List<Player>> Get();
        ActionResult<List<Player>> Get(string firstName, string lastName);
        ActionResult<Player> Get(string id);
    }
}