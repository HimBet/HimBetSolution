using System;
using System.Collections.Generic;
using HemBit.Model;
 

using MongoDB.Driver;

namespace HemBit.Services
{
    public class PlayerDBService : HemBitDBService<Player>
    {
        public PlayerDBService(IHemBitDatabaseSettings settings) : base(settings)
        {

        }

        public List<Player> Get(string firstName, string lastName) =>
            _items.Find<Player>(item => ((Player)item).FirstName == firstName
                                     && ((Player)item).LastName == lastName).ToList();
    }
}
