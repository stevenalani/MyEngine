using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine.XMLMaps
{
    public class OSM
    {
        public OSM() { }
        public List<OSMTag> Tags { get; set; }
    }
    public class OSMTag
    {
        public OSMTag() { }

        public OSMTag(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class OSMBound
    {
        public OSMBound() { }

        public OSMBound(Vector2 leftBottom, Vector2 rightTop, string filePath)
        {
            string fileRoot = filePath;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
