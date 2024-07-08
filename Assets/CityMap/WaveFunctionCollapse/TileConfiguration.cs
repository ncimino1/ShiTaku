namespace CityMap.WaveFunctionCollapse
{
    public class TileConfiguration
    {
        public TileTypes Type { get; }

        //[x][] where x is number of edges each with an array
        //storing the tile types that can be adjacent to that edge
        //    0
        // 3     1
        //    2
        private readonly TileTypes[][] _edges;

        public static TileConfiguration[] Generate()
        {
            TileConfiguration[] config = new TileConfiguration[20];
            config[0] = new TileConfiguration(TileTypes.RoadVertical,
                TileTypes.RoadVertical.GetPossibleAdjacentTiles());
            config[1] = new TileConfiguration(TileTypes.RoadHorizontal,
                TileTypes.RoadHorizontal.GetPossibleAdjacentTiles());
            config[2] = new TileConfiguration(TileTypes.Road4WayIntersection,
                TileTypes.Road4WayIntersection.GetPossibleAdjacentTiles());
            config[3] = new TileConfiguration(TileTypes.RoadCornerBR,
                TileTypes.RoadCornerBR.GetPossibleAdjacentTiles());
            config[4] = new TileConfiguration(TileTypes.RoadCornerBL,
                TileTypes.RoadCornerBL.GetPossibleAdjacentTiles());
            config[5] = new TileConfiguration(TileTypes.RoadCornerTR,
                TileTypes.RoadCornerTR.GetPossibleAdjacentTiles());
            config[6] = new TileConfiguration(TileTypes.RoadCornerTL,
                TileTypes.RoadCornerTL.GetPossibleAdjacentTiles());
            config[7] = new TileConfiguration(TileTypes.Road3WayBRT,
                TileTypes.Road3WayBRT.GetPossibleAdjacentTiles());
            config[8] = new TileConfiguration(TileTypes.Road3WayBLT,
                TileTypes.Road3WayBLT.GetPossibleAdjacentTiles());
            config[9] = new TileConfiguration(TileTypes.Road3WayLBR,
                TileTypes.Road3WayLBR.GetPossibleAdjacentTiles());
            config[10] = new TileConfiguration(TileTypes.Road3WayLTR,
                TileTypes.Road3WayLTR.GetPossibleAdjacentTiles());
            config[11] = new TileConfiguration(TileTypes.Park,
                TileTypes.Park.GetPossibleAdjacentTiles());
            config[12] = new TileConfiguration(TileTypes.SkyscraperCornerBL,
                TileTypes.SkyscraperCornerBL.GetPossibleAdjacentTiles());
            config[13] = new TileConfiguration(TileTypes.SkyscraperCornerBR,
                TileTypes.SkyscraperCornerBR.GetPossibleAdjacentTiles());
            config[14] = new TileConfiguration(TileTypes.SkyscraperCornerTL,
                TileTypes.SkyscraperCornerTL.GetPossibleAdjacentTiles());
            config[15] = new TileConfiguration(TileTypes.SkyscraperCornerTR,
                TileTypes.SkyscraperCornerTR.GetPossibleAdjacentTiles());
            config[16] = new TileConfiguration(TileTypes.PoliceStation,
                TileTypes.PoliceStation.GetPossibleAdjacentTiles());
            config[17] = new TileConfiguration(TileTypes.FireStation,
                TileTypes.FireStation.GetPossibleAdjacentTiles());
            config[18] = new TileConfiguration(TileTypes.CityHall,
                TileTypes.CityHall.GetPossibleAdjacentTiles());
            config[19] = new TileConfiguration(TileTypes.House,
                TileTypes.House.GetPossibleAdjacentTiles());

            return config;
        }

        public TileConfiguration(TileTypes type, TileTypes[][] edges)
        {
            Type = type;
            _edges = edges;
        }

        public TileTypes[] GetUp()
        {
            return _edges[0];
        }

        public TileTypes[] GetRight()
        {
            return _edges[1];
        }

        public TileTypes[] GetDown()
        {
            return _edges[2];
        }

        public TileTypes[] GetLeft()
        {
            return _edges[3];
        }
    }
}