using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CityMap.WaveFunctionCollapse
{
    //Grid class based on implementation in https://github.com/kavinbharathii/wave-function-collapse/tree/main
    public class Grid
    {
        public Tile[,] TileGrid { get; private set; }

        private int _width;
        private int _height;

        private TileConfiguration[] _options;

        private int skyscraperCount = 0;

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
            TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL,
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

        public void FixDuplicates()
        {
            var cityHall = TileGrid.Cast<Tile>().Where(t => t.TileOptions[0].Type == TileTypes.CityHall).ToArray();
            var fireStation = TileGrid.Cast<Tile>().Where(t => t.TileOptions[0].Type == TileTypes.FireStation)
                .ToArray();
            var policeStation = TileGrid.Cast<Tile>().Where(t => t.TileOptions[0].Type == TileTypes.PoliceStation)
                .ToArray();

            var house = _options.First(t => t.Type == TileTypes.House);

            var cityHallRandom = Random.Range(0, cityHall.Length);
            var fireStationRandom = Random.Range(0, fireStation.Length);
            var policeStationRandom = Random.Range(0, policeStation.Length);

            for (int i = 0; i < cityHall.Length; i++)
            {
                if (i == cityHallRandom)
                {
                    cityHall[i].TileOptions[0] = new TileConfiguration(TileTypes.CityHallDestroyed);
                    continue;
                }

                cityHall[i].TileOptions[0] = house;
            }

            for (int i = 0; i < fireStation.Length; i++)
            {
                if (i == fireStationRandom)
                {
                    fireStation[i].TileOptions[0] = new TileConfiguration(TileTypes.FireStationDestroyed);
                    continue;
                }

                fireStation[i].TileOptions[0] = house;
            }

            for (int i = 0; i < policeStation.Length; i++)
            {
                if (i == policeStationRandom)
                {
                    policeStation[i].TileOptions[0] = new TileConfiguration(TileTypes.PoliceStationDestroyed);
                    continue;
                }

                policeStation[i].TileOptions[0] = house;
            }

            var houseCount = TileGrid.Cast<Tile>().Count(t => t.TileOptions[0].Type == TileTypes.House);
            var parkCount = TileGrid.Cast<Tile>().Count(t => t.TileOptions[0].Type == TileTypes.Park);

            var newParks = 0;

            if (parkCount > houseCount / 4)
            {
                newParks = parkCount - houseCount / 4;
            }

            var parks = TileGrid.Cast<Tile>().Where(t => t.TileOptions[0].Type == TileTypes.Park).ToArray();

            var randIndicies = new HashSet<int>();

            while (randIndicies.Count != newParks)
            {
                var randGen = Random.Range(0, parkCount);
                randIndicies.Add(randGen);
            }

            for (int i = 0; i < parkCount; i++)
            {
                if (randIndicies.Contains(i))
                {
                    parks[i].TileOptions[0] = house;
                }
            }
        }

        public void FixRoads()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var type = TileGrid[y, x].TileOptions[0].Type;
                    if (type >= TileTypes.RoadVertical && type <= TileTypes.Road3WayLTR)
                    {
                        var above = new HashSet<TileTypes>();
                        if (y > 0)
                        {
                            above.AddRange(TileGrid[y - 1, x].TileOptions[0].Type.GetPossibleAdjacentTiles()[2]);
                        }

                        var right = new HashSet<TileTypes>();
                        if (x < _width - 1)
                        {
                            right.AddRange(TileGrid[y, x + 1].TileOptions[0].Type.GetPossibleAdjacentTiles()[3]);
                        }

                        var down = new HashSet<TileTypes>();
                        if (y < _height - 1)
                        {
                            down.AddRange(TileGrid[y + 1, x].TileOptions[0].Type.GetPossibleAdjacentTiles()[0]);
                        }

                        var left = new HashSet<TileTypes>();
                        if (x > 0)
                        {
                            left.AddRange(TileGrid[y, x - 1].TileOptions[0].Type.GetPossibleAdjacentTiles()[1]);
                        }

                        above.IntersectWith(right);
                        above.IntersectWith(down);
                        above.IntersectWith(left);

                        if (above.Count != 0 && !above.Contains(type))
                        {
                            var selection = _options.First(t => t.Type == above.First());
                            TileGrid[y, x].TileOptions[0] = selection;
                            Debug.Log("fixed");
                        }
                    }
                }
            }
        }

        public void FixSkyscrapers()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var tile = TileGrid[y, x];
                    if (tile.TileOptions[0].Type == TileTypes.SkyscraperCornerBL)
                    {
                        var up = TileGrid[y - 1, x].TileOptions[0];
                        var right = TileGrid[y, x + 1].TileOptions[0];
                        var rightup = TileGrid[y - 1, x + 1].TileOptions[0];
                        if (up.Type != TileTypes.SkyscraperCornerTL || right.Type != TileTypes.SkyscraperCornerBR ||
                            rightup.Type != TileTypes.SkyscraperCornerTR)
                        {
                            var house = new TileConfiguration(TileTypes.House);
                            tile.TileOptions[0] = house;
                            up = house;
                            right = house;
                            rightup = house;
                        }
                        else
                        {
                            skyscraperCount++;
                        }
                    }
                    else if (tile.TileOptions[0].Type == TileTypes.SkyscraperCornerBR)
                    {
                        var up = TileGrid[y - 1, x].TileOptions[0];
                        var left = TileGrid[y, x - 1].TileOptions[0];
                        var leftup = TileGrid[y - 1, x - 1].TileOptions[0];
                        if (up.Type != TileTypes.SkyscraperCornerTR || left.Type != TileTypes.SkyscraperCornerBL ||
                            leftup.Type != TileTypes.SkyscraperCornerTL)
                        {
                            var house = new TileConfiguration(TileTypes.House);
                            tile.TileOptions[0] = house;
                            up = house;
                            left = house;
                            leftup = house;
                        }
                        else
                        {
                            skyscraperCount++;
                        }
                    }
                    else if (tile.TileOptions[0].Type == TileTypes.SkyscraperCornerTL)
                    {
                        var down = TileGrid[y + 1, x].TileOptions[0];
                        var right = TileGrid[y, x + 1].TileOptions[0];
                        var rightdown = TileGrid[y + 1, x + 1].TileOptions[0];
                        if (down.Type != TileTypes.SkyscraperCornerBL || right.Type != TileTypes.SkyscraperCornerTR ||
                            rightdown.Type != TileTypes.SkyscraperCornerBR)
                        {
                            var house = new TileConfiguration(TileTypes.House);
                            tile.TileOptions[0] = house;
                            down = house;
                            right = house;
                            rightdown = house;
                        }
                        else
                        {
                            skyscraperCount++;
                        }
                    }
                    else if (tile.TileOptions[0].Type == TileTypes.SkyscraperCornerTR)
                    {
                        var down = TileGrid[y + 1, x].TileOptions[0];
                        var left = TileGrid[y, x - 1].TileOptions[0];
                        var leftdown = TileGrid[y + 1, x - 1].TileOptions[0];
                        if (down.Type != TileTypes.SkyscraperCornerBR || left.Type != TileTypes.SkyscraperCornerTL ||
                            leftdown.Type != TileTypes.SkyscraperCornerBL)
                        {
                            var house = new TileConfiguration(TileTypes.House);
                            tile.TileOptions[0] = house;
                            down = house;
                            left = house;
                            leftdown = house;
                        }
                        else
                        {
                            skyscraperCount++;
                        }
                    }
                }
            }
        }

        public void DestroyBuildings()
        {
            var houses = TileGrid.Cast<Tile>().Where(t => t.TileOptions[0].Type == TileTypes.House).ToArray();
            var houseCount = houses.Length;
            var destroyedCount = (houseCount / 2) - 1;
            destroyedCount = destroyedCount < 0 ? 0 : destroyedCount;

            var randIndicies = new HashSet<int>();

            while (randIndicies.Count != destroyedCount)
            {
                var randGen = Random.Range(0, houseCount);
                randIndicies.Add(randGen);
            }

            var destroyedHouse = new TileConfiguration(TileTypes.HouseDestroyed);

            for (int i = 0; i < houseCount; i++)
            {
                if (randIndicies.Contains(i))
                {
                    if (randIndicies.Count == 1)
                    {
                        houses[i].TileOptions[0] = new TileConfiguration(TileTypes.HardwareStoreDestroyed);
                        randIndicies.Remove(i);
                    }
                    else
                    {
                        houses[i].TileOptions[0] = destroyedHouse;
                        randIndicies.Remove(i);
                    }
                }
            }

            var parks = TileGrid.Cast<Tile>().Where(t => t.TileOptions[0].Type == TileTypes.Park).ToArray();
            var parkCount = parks.Length;
            destroyedCount = (parkCount / 2) - 1;
            destroyedCount = destroyedCount < 0 ? 0 : destroyedCount;

            randIndicies.Clear();

            while (randIndicies.Count != destroyedCount)
            {
                randIndicies.Add(Random.Range(0, parkCount));
            }

            var destroyedPark = new TileConfiguration(TileTypes.ParkDestroyed);
            for (int i = 0; i < parkCount; i++)
            {
                if (randIndicies.Contains(i))
                {
                    parks[i].TileOptions[0] = destroyedPark;
                }
            }

            if (skyscraperCount == 0)
                return;

            var destroyedSkyscraperCount = 0;

            if (skyscraperCount / 4 == 1)
            {
                destroyedSkyscraperCount = 1;
            }
            else
            {
                destroyedSkyscraperCount = Random.Range(1, skyscraperCount / 4);
            }

            randIndicies.Clear();

            while (randIndicies.Count != destroyedSkyscraperCount)
            {
                randIndicies.Add(Random.Range(0, skyscraperCount / 4));
            }

            var index = 0;
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    if (TileGrid[y, x].TileOptions[0].Type == TileTypes.SkyscraperCornerBL)
                    {
                        if (randIndicies.Contains(index))
                        {
                            TileGrid[y, x].TileOptions[0] =
                                new TileConfiguration(TileTypes.SkyscraperCornerBLDestroyed);
                            TileGrid[y - 1, x].TileOptions[0] =
                                new TileConfiguration(TileTypes.SkyscraperCornerTLDestroyed);
                            TileGrid[y, x + 1].TileOptions[0] =
                                new TileConfiguration(TileTypes.SkyscraperCornerBRDestroyed);
                            TileGrid[y - 1, x + 1].TileOptions[0] =
                                new TileConfiguration(TileTypes.SkyscraperCornerTRDestroyed);
                        }

                        index++;
                    }
                }
            }
        }

        public (int, int) GetRandomRoad()
        {
            var roads = TileGrid.Cast<Tile>().Where(t =>
                    t.TileOptions[0].Type >= TileTypes.RoadVertical && t.TileOptions[0].Type <= TileTypes.Road3WayLTR)
                .ToArray();

            var count = roads.Length;
            var randomRoad = Random.Range(0, count);
            var i = -1;
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    var type = TileGrid[y, x].TileOptions[0].Type;
                    if (type >= TileTypes.RoadVertical && type <= TileTypes.Road3WayLTR)
                    {
                        i++;
                    }

                    if (i == randomRoad)
                    {
                        return (x, y);
                    }
                }
            }

            return (0, 0);
        }
    }
}