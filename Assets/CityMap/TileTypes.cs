using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileTypes
{
    RoadVertical = 0,
    RoadHorizontal,
    Road4WayIntersection,
    RoadCornerBR,
    RoadCornerBL,
    RoadCornerTR,
    RoadCornerTL,
    Road3WayBRT,
    Road3WayBLT,
    Road3WayLBR,
    Road3WayLTR,
    Park,
    SkyscraperCornerBL,
    SkyscraperCornerBR,
    SkyscraperCornerTL,
    SkyscraperCornerTR,
    PoliceStation,
    FireStation,
    CityHall,
    House,
}

public static class TileTypesMethods
{
    private static readonly Dictionary<TileTypes, Color> TileToColor = new Dictionary<TileTypes, Color>()
    {
        { TileTypes.RoadVertical, Color.black },
        { TileTypes.RoadHorizontal, Color.blue },
        { TileTypes.Road4WayIntersection, Color.cyan },
        { TileTypes.RoadCornerBR, Color.gray },
        { TileTypes.RoadCornerBL, Color.gray },
        { TileTypes.RoadCornerTR, Color.gray },
        { TileTypes.RoadCornerTL, Color.gray },
        { TileTypes.Road3WayBRT, new Color(0.1f, 0.6f, 0.9f) },
        { TileTypes.Road3WayBLT, new Color(0.1f, 0.6f, 0.9f) },
        { TileTypes.Road3WayLBR, new Color(0.1f, 0.6f, 0.9f) },
        { TileTypes.Road3WayLTR, new Color(0.1f, 0.6f, 0.9f) },
        { TileTypes.Park, Color.magenta },
        { TileTypes.SkyscraperCornerBL, Color.red },
        { TileTypes.SkyscraperCornerBR, Color.red },
        { TileTypes.SkyscraperCornerTL, Color.red },
        { TileTypes.SkyscraperCornerTR, Color.red },
        { TileTypes.PoliceStation, Color.white },
        { TileTypes.FireStation, Color.yellow },
        { TileTypes.CityHall, new Color(0, 1, 0) },
        { TileTypes.House, new Color(0.5f, 0.25f, 0.25f) },
    };

    public static Color GetColor(this TileTypes tile)
    {
        return TileToColor[tile];
    }

    public static TileTypes[][] GetPossibleAdjacentTiles(this TileTypes tile)
    {
        TileTypes[][] adjacent = new TileTypes[4][];
        switch (tile)
        {
            case TileTypes.RoadVertical:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerBL, TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLBR,
                };

                adjacent[1] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerTR,
                    TileTypes.RoadCornerTL, TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLTR,
                };

                adjacent[3] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };
                break;
            case TileTypes.RoadHorizontal:
                adjacent[0] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBL,
                    TileTypes.RoadCornerTL,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
                };

                adjacent[2] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerTR,
                    TileTypes.Road3WayBRT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
                };
                break;
            case TileTypes.Road4WayIntersection:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal,
                };

                break;
            case TileTypes.RoadCornerBR:
                adjacent[0] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayLTR,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical,
                    TileTypes.Road3WayBLT,
                };

                adjacent[3] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerTR, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.RoadCornerBL:
                adjacent[0] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerTL, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical,
                    TileTypes.Road3WayBRT,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal,
                    TileTypes.Road3WayLTR,
                };

                break;
            case TileTypes.RoadCornerTR:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical,
                    TileTypes.Road3WayBLT,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal,
                    TileTypes.Road3WayLBR,
                };

                adjacent[2] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.RoadCornerTL:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical,
                    TileTypes.Road3WayBRT,
                };

                adjacent[1] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerTL, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal,
                    TileTypes.Road3WayLBR,
                };

                break;
            case TileTypes.Road3WayBRT:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical,
                    TileTypes.RoadCornerBL,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical,
                    TileTypes.RoadCornerTL,
                };

                adjacent[3] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.Road3WayBLT:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBR,
                };

                adjacent[1] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerTL, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerTR,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal,
                };

                break;
            case TileTypes.Road3WayLBR:
                adjacent[0] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal,
                    TileTypes.RoadCornerTL,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal,
                    TileTypes.RoadCornerTR,
                };

                break;
            case TileTypes.Road3WayLTR:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBL,
                };

                adjacent[2] = new[]
                {
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR,
                };

                break;
            case TileTypes.Park:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.SkyscraperCornerBL:
                adjacent[0] = new[]
                {
                    TileTypes.SkyscraperCornerTL,
                };

                adjacent[1] = new[]
                {
                    TileTypes.SkyscraperCornerBR,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.SkyscraperCornerBR:
                adjacent[0] = new[]
                {
                    TileTypes.SkyscraperCornerTR,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.SkyscraperCornerBL,
                };

                break;
            case TileTypes.SkyscraperCornerTL:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.SkyscraperCornerTR,
                };

                adjacent[2] = new[]
                {
                    TileTypes.SkyscraperCornerBL,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.SkyscraperCornerTR:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.SkyscraperCornerBR,
                };

                adjacent[3] = new[]
                {
                    TileTypes.SkyscraperCornerTL,
                };

                break;
            case TileTypes.PoliceStation:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.FireStation:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.CityHall:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.House:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
                    TileTypes.Park, TileTypes.SkyscraperCornerTL, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
        }

        return adjacent;
    }
}