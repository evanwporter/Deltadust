using System.IO;
using System.Xml.Serialization;

namespace Deltadust.Tiled
{
    public class Loader
    {
        public static TiledMap LoadMap(string filePath)
        {
            XmlSerializer serializer = new(typeof(TiledMap));
            using FileStream fs = new(filePath, FileMode.Open);
            TiledMap map = (TiledMap)serializer.Deserialize(fs);
            map.ParseTileLayers();
            return map;
        }
    }
}