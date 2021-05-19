using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Monte.Game
{
    public class Snake
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        
        //[JsonPropertyName("name")]
        //public string Name { get; set; }
        
        [JsonPropertyName("health")]
        public int Health { get; set; }
        
        [JsonPropertyName("body")]
        public List<Point> Body { get; set; }
        
        //[JsonPropertyName("latency")]
        //public int Latency { get; set; }
        
        [JsonPropertyName("head")]
        public Point Head { get; set; }
        
        public int Length => Body.Count;
        
        //[JsonPropertyName("shout")]
        //public string Shout { get; set; }
        
        //[JsonPropertyName("squad")]
        //public string Squad { get; set; }
    }
}