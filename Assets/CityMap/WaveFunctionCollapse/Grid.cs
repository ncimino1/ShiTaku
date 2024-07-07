using System.Collections.Generic;

namespace CityMap.WaveFunctionCollapse
{
    public class Grid
    {
        public TileTypes[,] TileGrid { get; }

        private int _width;
        private int _height;
        private Dictionary<TileTypes, Tile> _tiles;

        public Grid(int width, int height, Tile[] tiles)
        {
            TileGrid = new TileTypes[width, height];

            _tiles = new Dictionary<TileTypes, Tile>();

            _width = width;
            _height = height;

            foreach (var tile in tiles)
            {
                _tiles[tile.Type] = tile;
            }
        }
    }
}