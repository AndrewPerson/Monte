using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Monte.Game
{
    public class Board
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }
        
        [JsonPropertyName("width")]
        public int Width { get; set; }
        
        [JsonPropertyName("food")]
        public List<Point> Food { get; set; }
        
        //[JsonPropertyName("hazards")]
        //public Point[] Hazards { get; set; }
        
        [JsonPropertyName("snakes")]
        public List<Snake> Snakes { get; set; }
    }
}