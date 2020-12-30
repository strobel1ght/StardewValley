﻿using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Locations;

namespace ConvenientChests.CategorizeChests
{
    static class Utility
    {
        public static int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
                while (enumerator.MoveNext())
                    yield return YieldBatchElements(enumerator, batchSize - 1);
        }

        private static IEnumerable<T> YieldBatchElements<T>(
            IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;
            for (int i = 0; i < batchSize && source.MoveNext(); i++)
                yield return source.Current;
        }

        public static IDictionary<TKey, IEnumerable<TValue>> KeyBy<TKey, TValue>(this IEnumerable<TValue> values,
            Func<TValue, TKey> makeKey)
        {
            var dict = new Dictionary<TKey, IEnumerable<TValue>>();

            foreach (var value in values)
            {
                var key = makeKey(value);

                if (!dict.ContainsKey(key))
                    dict[key] = new List<TValue>();

                ((List<TValue>) dict[key]).Add(value);
            }

            return dict;
        }
        
        /// <summary>Get all game locations.</summary>
        public static IEnumerable<GameLocation> GetLocations()
        {
            return Game1.locations
                .Concat(
                    from location in Game1.locations.OfType<BuildableGameLocation>()
                    from building in location.buildings
                    where building.indoors.Value != null
                    select building.indoors.Value
                );
        }
    }
}