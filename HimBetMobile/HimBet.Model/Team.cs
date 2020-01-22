using System;
using Newtonsoft.Json;

namespace HemBit.Model
{
    public class Team : IPersistant
    {

        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Code")]
        public string Code { get; set; }
        public SportType SportType { get; set; }
        public Team() { } 
    }
}
