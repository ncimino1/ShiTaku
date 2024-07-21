using System.Linq;
using UnityEngine;

namespace CityMap.WaveFunctionCollapse
{
    //Tile class based on implementation in https://github.com/kavinbharathii/wave-function-collapse/tree/main
    public class Tile
    {
        public TileConfiguration[] TileOptions { get; set; }

        public bool Collapsed { get; private set; }

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
                Debug.Log("only roads :(");
                TileOptions = new[] { TileOptions[Random.Range(0, GetEntropy())] };
            }
            else
            {
                if (Random.Range(0, 4) == 3)
                {
                    TileOptions = new[] { sorted[Random.Range(0, roads)] };
                }
                else
                {
                    TileOptions = new[] { sorted[Random.Range(roads, sorted.Length)] };
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