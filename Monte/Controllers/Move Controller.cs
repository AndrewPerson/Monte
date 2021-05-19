using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monte.Game;
using Monte.Carlo;

namespace Monte.Moves
{
    public class MoveController : ControllerBase
    {
        [HttpPost("/move")]
        public async Task<Move> Move()
        {
            var buffer = await Request.BodyReader.ReadAsync();
            
            var root = (await JsonDocument.ParseAsync(Request.Body)).RootElement;

            var board = JsonSerializer.Deserialize<Board>(root.GetProperty("board").GetRawText());
            var me = JsonSerializer.Deserialize<Snake>(root.GetProperty("you").GetRawText());

            var futures = await Search.PropagateForwards(me.Length, me, board);

            return new Move
            {
                direction = await Search.GetBest(futures, me.ID)
            };
        }
    }
}