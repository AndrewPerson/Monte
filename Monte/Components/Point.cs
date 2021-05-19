using System.Text.Json.Serialization;

namespace Monte.Game
{
    public class Point
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        
        [JsonPropertyName("y")]
        public int Y { get; set; }
    }
}