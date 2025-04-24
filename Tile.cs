using Microsoft.Xna.Framework;

namespace MAZEGAME
{
    // Represents a section of the map - a tile can be of certain types and have a position
    public class Tile
    {
        public enum TileType { None, Wall, Ghost, GhostHouse, Player, Pellet, PowerPellet};
        public TileType tileType;
        public Vector2 position;

        public Tile(Vector2 position, TileType tileType)
        {
            this.tileType = tileType; // The type of tile (e.g., wall, ghost, etc.)
            this.position = position; // The position of the tile in the maze   
        }

        public Vector2 getPosition() // Returns the position of the tile
        {
            return position; 
        }
    }
}