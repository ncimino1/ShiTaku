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
                    TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.RoadVertical, TileTypes.Road3WayBRT,
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
                    TileTypes.RoadVertical, TileTypes.RoadCornerBL, TileTypes.RoadCornerTL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerTR, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };
                break;
            case TileTypes.RoadHorizontal:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
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
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
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
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerBL,
                    TileTypes.Road3WayBRT, TileTypes.Road3WayBLT, TileTypes.Road3WayLBR,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBL,
                    TileTypes.RoadCornerTL,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerTR,
                    TileTypes.RoadCornerTL,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLTR,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerTR,
                    TileTypes.Road3WayBRT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
                };

                break;
            case TileTypes.RoadCornerBR:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerBL, TileTypes.PoliceStation,
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
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerTL,
                    TileTypes.RoadCornerTR,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLTR,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerTL, TileTypes.RoadCornerBL, TileTypes.Road3WayBLT,
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerTR, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.RoadCornerBL:
                adjacent[0] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerTR, TileTypes.RoadCornerTL, TileTypes.Road3WayLTR,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerBR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerTL, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                adjacent[2] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerTL,
                    TileTypes.RoadCornerTR,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLTR,
                };

                adjacent[3] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerTR,
                    TileTypes.Road3WayBRT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
                };

                break;
            case TileTypes.RoadCornerTR:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerBL,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLBR,
                };

                adjacent[1] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBL,
                    TileTypes.RoadCornerTL,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayLBR, TileTypes.Road3WayLTR,
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
                    TileTypes.Park, TileTypes.SkyscraperCornerBR, TileTypes.SkyscraperCornerTR, TileTypes.PoliceStation,
                    TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };

                break;
            case TileTypes.RoadCornerTL:
                adjacent[0] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.Road4WayIntersection, TileTypes.RoadCornerBR,
                    TileTypes.RoadCornerBL,
                    TileTypes.Road3WayBLT, TileTypes.Road3WayBRT, TileTypes.Road3WayLBR,
                };
                
                adjacent[1] = new[]
                {
                    TileTypes.RoadVertical, TileTypes.RoadCornerBR, TileTypes.RoadCornerTR, TileTypes.Road3WayBRT,
                    TileTypes.Park, TileTypes.SkyscraperCornerBL, TileTypes.SkyscraperCornerTL, TileTypes.Park,
                    TileTypes.PoliceStation, TileTypes.FireStation, TileTypes.CityHall, TileTypes.House,
                };
                
                adjacent[2] = new[]
                {
                    TileTypes.RoadHorizontal, TileTypes.RoadCornerBR, TileTypes.RoadCornerBL, TileTypes.Road3WayLBR,
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
            case TileTypes.Road3WayBRT:
                adjacent[0] = new []
                {
                    
                }
        }
    }
}