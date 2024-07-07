namespace CityMap.WaveFunctionCollapse
{
    public class Tile
    {
        public string Texture { get; }

        public TileTypes Type { get; }

        //[x][] where x is number of edges each with an array
        //storing the tile types that can be adjacent to that edge
        private readonly TileTypes[][] _edges;

        public Tile(string texture, TileTypes type, TileTypes[][] edges)
        {
            Texture = texture;
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