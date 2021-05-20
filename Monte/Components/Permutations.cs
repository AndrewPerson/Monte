using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace Monte.Carlo
{
    class Combinations
    {
        public static async Task RepeatingCombinations<T>(List<T> items, int chooseCount, Func<List<T>, Task> callback)
        {
            int itemCount = items.Count;
            int maxCount = itemCount - 1;
            int totalCount = (int) Math.Pow(itemCount, chooseCount);

            int[] indexes = new int[chooseCount];

            List<T> perm = new List<T>(chooseCount);
            for (int i = 0; i < chooseCount; i++) perm.Add(items[0]);

            await callback(items);

            for (int n = 0; n < totalCount - 1; n++)
            {
                for (int i = 0; i < chooseCount; i++)
                {
                    if (indexes[i] == maxCount) indexes[i] = 0;
                    else indexes[i]++;

                    perm[i] = items[indexes[i]];
                }

                await callback(perm);
            }
        }
    }
}