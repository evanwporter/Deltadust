using System.Collections.Generic;

namespace Deltadust {
        public class MapData {
        public string MapName { get; set; }
        public List<NPCData> NPCs { get; set; }
        public List<MonsterData> Monsters { get; set; }
    }

    public class NPCData {
        public string Type { get; set; }
        public string Name { get; set; }
        public PositionData Position { get; set; }
        public string Dialogue { get; set; }
    }

    public class MonsterData {
        public string Type { get; set; }
        public string Name { get; set; }
        public PositionData Position { get; set; }
        public float Speed { get; set; }
    }

    public class PositionData {
        public float X { get; set; }
        public float Y { get; set; }
    }
}