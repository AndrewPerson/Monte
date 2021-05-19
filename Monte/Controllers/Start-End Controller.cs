using Microsoft.AspNetCore.Mvc;

namespace Monte.Game
{
    public class StartEndController : ControllerBase
    {
        private int activeGames = 0;
        
        [HttpPost("/start")]
        public void Start()
        {
            activeGames++;
        }

        [HttpPost("/end")]
        public void End()
        {
            activeGames--;
        }

        [HttpGet("/games")]
        public int Games()
        {
            return activeGames;
        }
    }
}