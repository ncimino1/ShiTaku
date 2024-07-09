using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CityMap.WaveFunctionCollapse
{
    public class Grid
    {
        public Tile[,] TileGrid { get; private set; }

        private int _width;
        private int _height;

        private TileConfiguration[] _options;

        private static HashSet<TileTypes> _rightEdgeForbidden = new HashSet<TileTypes>()
        {
            TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
            TileTypes.RoadCornerTR, TileTypes.Road3WayBRT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
            TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerTL,
        };

        private static HashSet<TileTypes> _leftEdgeForbidden = new HashSet<TileTypes>()
        {
            TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBL,
            TileTypes.RoadCornerTL, TileTypes.Road3WayBLT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
            TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerTR,
        };

        private static HashSet<TileTypes> _topEdgeForbidden = new HashSet<TileTypes>()
        {
            TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerTL, TileTypes.RoadCornerTR,
            TileTypes.Road3WayBRT, TileTypes.Road3WayBLT, TileTypes.Road3WayLTR, TileTypes.SkyscraperCornerBL,
            TileTypes.SkyscraperCornerBR
        };

        private static HashSet<TileTypes> _bottomEdgeForbidden = new HashSet<TileTypes>()
        {
            TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL,
            TileTypes.Road3WayBRT, TileTypes.Road3WayBLT, TileTypes.Road3WayLBR, TileTypes.SkyscraperCornerTL,
            TileTypes.SkyscraperCornerTR,
        };

        public Grid(int width, int height, TileConfiguration[] tiles)
        {
            TileGrid = new Tile[height, width];

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

        private TileConfiguration[] FixEdgeTile(Tile tile, int x, int y, TileConfiguration[] configuration)
        {
            var newConfig = new List<TileConfiguration>();
            foreach (var config in configuration)
            {
                if (x == 0 && _leftEdgeForbidden.Contains(config.Type))
                    continue;
                if (x == _width - 1 && _rightEdgeForbidden.Contains(config.Type))
                    continue;
                if (y == 0 && _topEdgeForbidden.Contains(config.Type))
                    continue;
                if (y == _height - 1 && _bottomEdgeForbidden.Contains(config.Type))
                    continue;

                newConfig.Add(config);
            }

            return newConfig.ToArray();
        }

        public bool Collapse()
        {
            var pick = HeuristicPickTile();

            if (pick == null)
            {
                return true;
            }

            pick.ObserveTile();

            Tile[,] shallowCopy = (Tile[,])TileGrid.Clone();

            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
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

                        if (i == 0 || i == _height - 1 || j == 0 || j == _width - 1)
                            cumulative_options = FixEdgeTile(shallowCopy[i, j], j, i, cumulative_options);

                        if (cumulative_options.Length == 0)
                            cumulative_options = new[] { _options.First(t => t.Type == TileTypes.House) };


                        shallowCopy[i, j].TileOptions = cumulative_options;
                        shallowCopy[i, j].Update();
                    }
                }
            }

            TileGrid = shallowCopy;

            return false;
        }
    }
}