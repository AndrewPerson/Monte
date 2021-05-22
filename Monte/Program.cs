using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Monte.Carlo;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Monte.Moves;
using Monte.Game;

namespace Monte
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
            List<Direction> directions = new List<Direction> {
                Direction.Up,
                Direction.Down,
                Direction.Left,
                Direction.Right
            };

            await Combinations.RepeatingCombinations(directions, 2, async list => {
                var result = "INFO: ";
                foreach (var item in list)
                {
                    result += item.ToString() + " ";
                }
                Console.WriteLine(result);
            });
            */

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}