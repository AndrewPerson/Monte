using System.Text.Json.Serialization;

namespace Monte.Game
{
    public class Point
    {
        [JsonPropertyName("x")]
        public int X { get; set; }
        
        [JsonPropertyName("y")]
        public int Y { get; set; }

        public static bool operator == (Point self, Point other)
        {
            return self.Equals(other);
        }

        public static bool operator != (Point self, Point other)
        {
            return !(self == other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (!(obj is Point)) return false;
            Point p = (Point) obj;
            return p.X == X && p.Y == Y;
        }

        public override int GetHashCode()
        {
            return this.X ^ this.Y;
        }
    }
}