using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace MyEngine.XMLMaps
{
    public class OsmOject
    {
        public List<Item> data;
    }
    public class Item
    {
        public string latitude;
        public string elevation;
        public string longitude;
    }

    public class OSM
    {
        public float latitude { get; set; }
        public float elevation { get; set; }
        public float longitude { get; set; }
        public OSM() { }    
        public string OSMBound(Vector2 leftBottom, Vector2 rightTop, string filePath)
        {
            string[] fileEntries = Directory.GetFiles(filePath); 
            foreach (var file in fileEntries)
            {
                //focused BBoxe
            }

            return "";
        }
    }
}
