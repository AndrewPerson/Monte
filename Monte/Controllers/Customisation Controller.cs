using Microsoft.AspNetCore.Mvc;

namespace Monte.Customisation
{
    public class CustomizationController : ControllerBase
    {
        [HttpGet("/")]
        public Customisation Get()
        {
            return new ()
            {
                apiversion = 1,
                author = "AndrewPerson",
                color = "#8599ff",
                head = "bendr",
                tail = "bolt"
            };
        }
    }
}