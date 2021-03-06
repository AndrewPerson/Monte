using System.Text.Json.Serialization;

namespace Monte.Moves
{
    public enum Direction
    {
         Up,
         Down,
         Left,
         Right
    }
    
    public class Move
    {
        public Direction direction;

        [JsonPropertyName("move")]
        public string Movement
        {
            get
            {
                if (direction == Direction.Up) return "up";
                if (direction == Direction.Down) return "down";
                if (direction == Direction.Left) return "left";
                if (direction == Direction.Right) return "right";
                return "up";
            }

            set
            {
                var v = value.ToLower();
                if (v == "up") direction = Direction.Up;
                else if (v == "down") direction = Direction.Down;
                else if (v == "left") direction = Direction.Left;
                else if (v == "right") direction = Direction.Right;
            }
        }

        [JsonPropertyName("shout")]
        public string Shout { get; set; }
    }
}