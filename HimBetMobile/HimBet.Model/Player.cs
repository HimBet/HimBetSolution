using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HemBit.Model
{
    public class Player : IPersistant
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageURL { get; set; }
        public SportType SportType { get; set; }
        //public DateTime DateOfBirth { get; set; }
        public List<PlayingPeriod> Teams { get; set; }
    } 
}
