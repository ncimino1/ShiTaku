using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace CityMap.WaveFunctionCollapse
{
    public class Grid
    {
        public Tile[,] TileGrid { get; private set; }

        private int _width;
        private int _height;

        private TileConfiguration[] _options;

        public Grid(int width, int height, TileConfiguration[] tiles)
        {
            TileGrid = new Tile[width, height];

            _width = width;
            _height = height;

            _options = tiles;

            for (int i = 0; i < TileGrid.GetLength(0); i++)
            {
                for (int j = 0; j < TileGrid.GetLength(1); j++)
                {
                    TileGrid[i, j] = new Tile(tiles);
                }
            }
        }

        private Tile HeuristicPickTile()
        {
            Tile[] tileGridCopy = TileGrid.Cast<Tile>().Select(i => i).Where(i => i.GetEntropy() > 1)
                .OrderBy(i => i.GetEntropy()).ToArray();

            if (tileGridCopy.Length == 0)
                return null;

            var initial = tileGridCopy[0];
            tileGridCopy = tileGridCopy.Where(i => i.GetEntropy() == initial.GetEntropy()).ToArray();

            if (tileGridCopy.Length == 0)
                return null;

            return tileGridCopy[Random.Range(0, tileGridCopy.Length)];
        }

        public void Collapse()
        {
            var pick = HeuristicPickTile();

            if (pick == null)
                return;

            pick.ObserveTile();

            Tile[,] shallowCopy = (Tile[,])TileGrid.Clone();

            for (int i = 0; i < shallowCopy.GetLength(0); i++)
            {
                for (int j = 0; j < shallowCopy.GetLength(1); j++)
                {
                    if (TileGrid[i, j].Collapsed)
                    {
                        shallowCopy[i, j] = TileGrid[i, j];
                    }
                    else
                    {
                        var cumulative_options = _options;

                        //Up
                        if (i > 0)
                        {
                            var above = TileGrid[i - 1, j];
                            var valid = new List<TileTypes>();

                            foreach (var option in above.TileOptions)
                            {
                                valid.AddRange(option.GetDown().Select(t => t));
                            }

                            cumulative_options = cumulative_options.Where(t => valid.Contains(t.Type)).ToArray();
                        }

                        //Right
                        if (j < shallowCopy.GetLength(1) - 1)
                        {
                            var right = TileGrid[i, j + 1];
                            var valid = new List<TileTypes>();

                            foreach (var option in right.TileOptions)
                            {
                                valid.AddRange(option.GetLeft().Select(t => t));
                            }

                            cumulative_options = cumulative_options.Where(t => valid.Contains(t.Type)).ToArray();
                        }

                        //Down
                        if (i < shallowCopy.GetLength(0) - 1)
                        {
                            var right = TileGrid[i + 1, j];
                            var valid = new List<TileTypes>();

                            foreach (var option in right.TileOptions)
                            {
                                valid.AddRange(option.GetUp().Select(t => t));
                            }

                            cumulative_options = cumulative_options.Where(t => valid.Contains(t.Type)).ToArray();
                        }

                        //Left
                        if (j > 0)
                        {
                            var right = TileGrid[i, j - 1];
                            var valid = new List<TileTypes>();

                            foreach (var option in right.TileOptions)
                            {
                                valid.AddRange(option.GetRight().Select(t => t));
                            }

                            cumulative_options = cumulative_options.Where(t => valid.Contains(t.Type)).ToArray();
                        }

                        shallowCopy[i, j].TileOptions = cumulative_options;
                        shallowCopy[i, j].Update();
                    }
                }
            }

            TileGrid = shallowCopy;
        }
    }
}