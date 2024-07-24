using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace CityMap.WaveFunctionCollapse
{
    //Tile class based on implementation in https://github.com/kavinbharathii/wave-function-collapse/tree/main
    public class Tile
    {
        public TileConfiguration[] TileOptions { get; set; }

        public bool Collapsed { get; private set; }
        
        private static Random random = new Random();

        public Tile(TileConfiguration[] options)
        {
            TileOptions = options;
        }

        public int GetEntropy() => TileOptions.Length;

        public void Update()
        {
            Collapsed = GetEntropy() == 1;
        }

        public void ObserveTile()
        {
            if (TileOptions.Length == 0)
                return;

            int roads = 0;
            foreach (var option in TileOptions)
            {
                if (option.Type >= TileTypes.RoadVertical && option.Type <= TileTypes.Road3WayLTR)
                    roads++;
            }

            var sorted = TileOptions.OrderBy(t => t.Type).ToArray();

            var nonRoads = TileOptions.Length - roads;

            if (nonRoads == 0)
            {
                TileOptions = new[] { TileOptions[random.Next(0, GetEntropy())] };
            }
            else
            {
                if (random.Next(0, 4) == 3)
                {
                    TileOptions = new[] { sorted[random.Next(0, roads)] };
                }
                else
                {
                    TileOptions = new[] { sorted[random.Next(roads, sorted.Length)] };
                }
            }

            if (TileOptions.Length == 0)
            {
                TileOptions = new[]
                {
                    new TileConfiguration(TileTypes.House, TileTypes.House.GetPossibleAdjacentTiles())
                };
            }

            Collapsed = true;
        }
    }
}