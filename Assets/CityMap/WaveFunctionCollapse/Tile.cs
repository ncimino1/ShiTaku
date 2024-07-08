using UnityEngine;

namespace CityMap.WaveFunctionCollapse
{
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
            
            TileOptions = new[] { TileOptions[Random.Range(0, GetEntropy())] };
            Collapsed = true;
        }
    }
}