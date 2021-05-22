using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monte.Game;
using Monte.Carlo;
using System;

namespace Monte.Moves
{
    public class MoveController : ControllerBase
    {
        [HttpPost("/move")]
        public async Task<Move> Move()
        {
            var root = (await JsonDocument.ParseAsync(Request.BodyReader.AsStream())).RootElement;

            var board = JsonSerializer.Deserialize<Board>(root.GetProperty("board").GetRawText());
            var me = JsonSerializer.Deserialize<Snake>(root.GetProperty("you").GetRawText());

            var turn = root.GetProperty("turn").GetInt32();

            if (turn < 2)
            {
                if (me.Head.X > board.Width / 2) return new Move
                {
                    direction = Direction.Left
                };
                else if (me.Head.X < board.Width / 2) return new Move
                {
                    direction = Direction.Right
                };
                else if (me.Head.Y > board.Height / 2) return new Move
                {
                    direction = Direction.Down
                };
                else return new Move
                {
                    direction = Direction.Up
                };
            }

            var futures = await Search.LookForwards(me.Length, me, board);

            //Console.WriteLine("INFO: FUTURES: " + futures.Count);

            return new Move
            {
                direction = await Search.GetBest(futures, me.ID)
            };
        }
    }
}