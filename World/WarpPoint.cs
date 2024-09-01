using Microsoft.Xna.Framework;

namespace Deltadust.World {
    public class WarpPoint {
        public string Name { get; set; }
        public string MapName { get; set; }
        public Vector2 TargetPosition { get; set; }
        public Rectangle Bounds { get; set; }
    } 
}