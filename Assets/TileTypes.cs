using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes
{
    Road1 = 0,
    Road2 = 1,
    Road3 = 2,
    Road4 = 3,
    Park = 4,
    Skyscraper = 5,
    PoliceStation = 6,
    FireStation = 7,
    CityHall = 8,
    House = 9,
}

public static class TileTypesMethods
{
    private static readonly Dictionary<TileTypes, Color> TileToColor = new Dictionary<TileTypes, Color>()
    {
        { TileTypes.Road1, Color.black },
        { TileTypes.Road2, Color.blue },
        { TileTypes.Road3, Color.cyan },
        { TileTypes.Road4, Color.gray },
        { TileTypes.Park, Color.magenta },
        { TileTypes.Skyscraper, Color.red },
        { TileTypes.PoliceStation, Color.white },
        { TileTypes.FireStation, Color.yellow },
        { TileTypes.CityHall, new Color(0, 1, 0) },
        { TileTypes.House, new Color(0.5f, 0.25f, 0.25f) },
    };

    public static Color GetColor(this TileTypes tile)
    {
        return TileToColor[tile];
    }
}