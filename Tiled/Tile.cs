namespace Deltadust.Tiled
{
    public class Tile
    {
        public int Id { get; set; } // The tile's global ID
        public int X { get; set; }   // The X position in the grid
        public int Y { get; set; }   // The Y position in the grid

        public Tile(int id, int x, int y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public bool IsEmpty()
        {
            return Id == 0;  // Assuming 0 means an empty tile with no texture
        }

        public int GlobalIdentifier => Id;

    }
}
